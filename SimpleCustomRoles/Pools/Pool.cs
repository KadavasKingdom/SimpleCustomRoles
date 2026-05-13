using PlayerRoles;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using Utils.NonAllocLINQ;

namespace SimpleCustomRoles.Pools
{
    /*
        Pool comprised of custom roles (or null values) along with weights. Every role has a lower bound and an upper bound,
        and the difference between those two is its weight. When a custom role is randomized for, a random number is picked
        and finds the role where the random number is between the lower bound and upper bound. That role is returned, and
        removed from the pool.
    Example:
        Pool is comprised of: [(0, 100, NtfQRF), (100, 500, GuardSecretary), (500, 900, GuardSecretary), (900, 2900, null)]
        Pool is asked for a random role. It generates 0.12 from RNG, multiplies it by 2900, gets 345.
        Since 345 is between 100 and 500, Secretary is picked. It is removed from the pool. All custom roles are compacted to the left.
        Pool is now: [(0, 100, NtfQRF), (100, 500, GuardSecretary), (500, 2500, null)]
        Again rng rolls 0.95, which is 2375. That role is null, so null (no custom role) is returned, but the null value is not removed from the pool. 
    */
    public class Pool
    {
        // Role type for this pool. Can be None if the pool uses multiple role types.
        // (mainly used to determine the weight of no custom role spawning)
        RoleTypeId _RoleType;

        // Lower inclusive, upper exclusive. invariants: always ordered and contiguous,
        // Upper > Lower, Lower >= 0, Upper > 0. When used to get a random role, must not be empty
        internal List<(int Lower, int Upper, CustomRoleBaseInfo? Role)> roles = [];
        // Applies "adjustments" on the rng value rolled. Ex: rng returns 0.786,
        // adjustment funcs modify that to 0.675, and that is the number used for calculation.
        internal List<Func<float, float>> AdjustmentFuncs = new();
        // Creates a new pool, adds each new role into it, multiplying its spawn chance by what transformFunc returns for that role.
        internal Pool Transform(Func<CustomRoleBaseInfo?, float> transformFunc)
        {
            var pool = new Pool();

            pool.AdjustmentFuncs = AdjustmentFuncs;

            foreach (var entry in roles)
            {
                if (entry.Role == null)
                {
                    pool.AddRole(entry.Role);
                    continue;
                }

                float multiplier = transformFunc(entry.Role);

                // usually will multiply by 1, but must check anyways
                multiplier *= (entry.Upper - entry.Lower) / GetSpawnChance(entry.Role);

                if (multiplier <= 0f)
                    continue;



                int NewChance = (int)(GetSpawnChance(entry.Role) * multiplier);


                pool.AddRole(entry.Role, NewChance, 1);
            }

            return pool;
        }

        // transform using multiple transform funcs
        internal Pool Transform(IEnumerable<Func<CustomRoleBaseInfo?, float>> transformFuncs) =>
            Transform(x =>
            {
                float ret = 1f;
                foreach (var func in transformFuncs)
                    ret = (int)(ret * func(x));
                return ret;
            });

        // transform, but modify the original pool instead of making a new one
        internal void TransformInPlace(Func<CustomRoleBaseInfo?, float> transformFunc)
        {
            var new_pool = Transform(transformFunc);
            roles = new_pool.roles;
            AdjustmentFuncs = new_pool.AdjustmentFuncs;
        }

        internal void TransformInPlace(IEnumerable<Func<CustomRoleBaseInfo?, float>> transformFuncs)
        {
            var new_pool = Transform(transformFuncs);
            roles = new_pool.roles;
            AdjustmentFuncs = new_pool.AdjustmentFuncs;
        }

        public void AddAdjustmentFunc(Func<float, float> adjustmentFunc)
        {
            // add adjustment func with a wrapper to make sure rng value stays valid
            AdjustmentFuncs.Add(x =>
            {
                float num = adjustmentFunc(x);
                if (num < 0 || num >= 1)
                    throw new ArithmeticException($"Adjustment function returned an invalid number, should be [0, 1) but is instead {num}");

                return num;
            });
        }

        // Gets SpawnAmount for a role, multiplied by all configs
        int GetSpawnAmount(CustomRoleBaseInfo? role)
        {
            if (role == null)
                return 1;

            int SpawnAmount = role.Spawn.SpawnAmount;
            if (Main.Instance.Config.SpawnAmountDictionary.TryGetValue(SpawnAmount, out var newAmt))
                SpawnAmount = newAmt;

            if (role.Rolegroup != null && Main.Instance.RoleGroups.TryGetFirst(x => x.Name == role.Rolegroup, out var group) && group.SpawnAmountMultiplier != 1f)
                SpawnAmount = (int)(SpawnAmount * group.SpawnAmountMultiplier);
            else
                SpawnAmount = (int)(SpawnAmount * Main.Instance.Config.SpawnAmountMultiplier);

            return SpawnAmount;
        }

        // Gets SpawnChance for a role, multiplied by all configs
        int GetSpawnChance(CustomRoleBaseInfo? role)
        {
            // Config for weight of no custom role.
            if (role == null)
            {
                if (!Main.Instance.Config.NoCustomRoleChance.TryGetValue(_RoleType, out var CfgChance))
                    CfgChance = 2000;
                return CfgChance;
            }


            int Chance = role.Spawn.SpawnChance;

            if (role.Spawn.DenyChance)
                return Chance;

            if (role.ReplaceRole == RoleTypeId.Scp0492)
                Chance = role.Scp.Scp0492.ChanceForSpawn;

            if (role.Rolegroup != null && Main.Instance.RoleGroups.TryGetFirst(x => x.Name == role.Rolegroup, out var group) && group.SpawnChanceMultiplier != 1f)
                Chance = (int)(Chance * group.SpawnChanceMultiplier);
            else
                Chance = (int)(Chance * Main.Instance.Config.SpawnRateMultiplier);

            return Chance;
        }

        // Main entry point to add roles to the pool. useSpawnAmount might be false if you're copying in roles from another pool
        public void AddRole(CustomRoleBaseInfo? role, bool useSpawnAmount = true)
        {
            int SpawnAmount = useSpawnAmount ? GetSpawnAmount(role) : 1;
            int Chance = GetSpawnChance(role);

            AddRole(role, Chance, SpawnAmount);
        }

        // Add role with preset weight and amount, all logic is here
        public void AddRole(CustomRoleBaseInfo? role, int weight, int amount)
        {
            // If pool has a dedicated role type, don't allow other role types to be inserted
            if (_RoleType != RoleTypeId.None && role != null && role.ReplaceRole != _RoleType)
            {
                CL.Error($"Role replacing {role!.ReplaceRole} attempted to be added to a pool typed to replace {_RoleType}. Not inserting into pool.");
                return;
            }


            if (weight == 0)
                return;


            for (int i = 0; i < amount; i++)
            {
                int LowerBound = 0;

                // Get lower bound from the last item in roles
                if (!roles.IsEmpty())
                    LowerBound = roles.Last().Upper;

                int UpperBound = LowerBound + weight;


                roles.Add((LowerBound, UpperBound, role));
            }
        }

        // Put rare roles further left, and more common roles further right
        internal void SortByRarity()
        {
            int Cursor = 0;
            // Order by weight (but put the null values at the end)
            var ListByChance = roles.OrderBy(x => x.Role == null ? int.MaxValue : x.Upper - x.Lower).ToList();

            List<(int Lower, int Upper, CustomRoleBaseInfo? Role)> NewRoles = new();

            // Have to do this to maintain invariants in the roles
            foreach ((int Lower, int Upper, CustomRoleBaseInfo? Role) in ListByChance)
            {
                int Size = Upper - Lower;
                NewRoles.Add((Cursor, Cursor + Size, Role));
                Cursor += Size;
            }

            roles = NewRoles;
        }

        public CustomRoleBaseInfo? GetRandomRole()
        {
            // random float in [0-1)
            float randomNumber = RandomGenerator.GetUInt32(true) / 4294967296f;

            return GetRandomRole(randomNumber);
        }

        internal CustomRoleBaseInfo? GetRandomRole(float randomNumber)
        {
            if (randomNumber < 0 || randomNumber >= 1)
                throw new ArgumentException("random number must be between 0 and 1");

            float unadjustedRandom = randomNumber;

            foreach (var AdjustmentFunc in AdjustmentFuncs)
            {
                randomNumber = AdjustmentFunc(randomNumber);
            }

            if (roles.Count == 0)
            {
                return null;
            }

            // Convert random number to a weight in the pool
            int adjustedNumber = (int)(randomNumber * roles.Last().Upper);

            int rolePos = -1;

            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Lower <= adjustedNumber && roles[i].Upper > adjustedNumber)
                {
                    rolePos = i;
                    break;
                }
            }

            if (rolePos == -1)
            {
                CL.Error($"got -1 rolepos. {adjustedNumber}");
                return null;
            }

            // If role is invalid (e.g. blocked by group) get a different role and remove the invalid role from pool.
            if (!ValidateRole(rolePos))
            {
                RemoveRole(rolePos);
                return GetRandomRole(unadjustedRandom);
            }


            var role = RemoveRole(rolePos);


            return role;
        }

        // Checks whether the role at roles[RolePosition] is valid to be selected
        internal bool ValidateRole(int RolePosition)
        {
            var role = roles[RolePosition].Role;

            if (role == null)
                return true;

            if (!CustomRoleHelpers.IsShouldSpawn(role))
            {
                CL.Debug($"Role has been no longer spawn: {role.Rolename} (Reason: Player limited)", Main.Instance.Config.Debug);
                return false;
            }
            if (!GroupHelper.CanSpawn(role.Rolegroup, ref PoolManager.AlreadySpawnedRoles))
            {
                CL.Debug($"Role has been no longer spawn: {role.Rolename} (Reason: Group limited)", Main.Instance.Config.Debug);
                return false;
            }

            return true;
        }

        internal CustomRoleBaseInfo? RemoveRole(CustomRoleBaseInfo Role)
        {
            if (Role == null)
                return null;

            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Role == Role)
                {
                    return RemoveRole(i);
                }
            }

            return null;
        }

        internal CustomRoleBaseInfo? RemoveRole(int RolePos)
        {
            // Get role (it's about to be removed from the list)
            var role = roles[RolePos];

            if (role.Role == null)
                return null;
            // Get weight for selected role
            int middleChance = role.Upper - role.Lower;
            // Subtract weight from Lower and Upper for all roles to the right of selected role
            for (int i = RolePos + 1; i < roles.Count; i++)
            {
                int newLower = roles[i].Lower - middleChance;
                int newUpper = roles[i].Upper - middleChance;

                roles[i] = (newLower, newUpper, roles[i].Role);
            }
            roles.RemoveAt(RolePos);

            PoolManager.AlreadySpawnedRoles.Add(role.Role!);

            return role.Role!;
        }

        internal Pool(RoleTypeId ty = RoleTypeId.None)
        {
            _RoleType = ty;
        }
    }
}
