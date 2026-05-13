using PlayerRoles;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;
using Utils.NonAllocLINQ;

namespace SimpleCustomRoles.Pools
{
    public class Pool
    {
        RoleTypeId _RoleType;
        // Lower inclusive, upper exclusive
        internal List<(int Lower, int Upper, CustomRoleBaseInfo? Role)> roles = [];
        internal List<Func<float, float>> AdjustmentFuncs = new();
        internal Pool Transform(Func<CustomRoleBaseInfo?, float> transformFunc)
        {
            var pool = new Pool();

            pool.AdjustmentFuncs = AdjustmentFuncs;

            foreach (var entry in roles.Where(x => x.Role != null))
            {
                float multiplier = transformFunc(entry.Role);

                multiplier *= (entry.Upper - entry.Lower) / entry.Role!.Spawn.SpawnChance;

                if (multiplier == 0f)
                    continue;

                int LowerBound = 0;

                if (!pool.roles.IsEmpty())
                    LowerBound = pool.roles.Last().Upper;

                int NewChance = (int)(entry.Role.Spawn.SpawnChance * multiplier);


                pool.AddRole(entry.Role, NewChance, 1);
            }

            return pool;
        }


        internal Pool Transform(IEnumerable<Func<CustomRoleBaseInfo?, float>> transformFuncs) =>
            Transform(x =>
            {
                float ret = 1f;
                foreach (var func in transformFuncs)
                    ret = (int)(ret * func(x));
                return ret;
            });

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
            AdjustmentFuncs.Add(x =>
            {
                float num = adjustmentFunc(x);
                if (num < 0 || num >= 1)
                    throw new ArithmeticException($"Adjustment function returned an invalid number, should be [0, 1) but is instead {num}");

                return num;
            });
        }

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

        int GetSpawnChance(CustomRoleBaseInfo? role)
        {
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

        public void AddRole(CustomRoleBaseInfo? role, bool useSpawnAmount = true)
        {
            int SpawnAmount = useSpawnAmount ? GetSpawnAmount(role) : 1;
            int Chance = GetSpawnChance(role);

            AddRole(role, Chance, SpawnAmount);
        }

        public void AddRole(CustomRoleBaseInfo? role, int weight, int amount)
        {
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

                if (!roles.IsEmpty())
                    LowerBound = roles.Last().Upper;

                int UpperBound = LowerBound + weight;


                roles.Add((LowerBound, UpperBound, role));
            }
        }

        internal void SortByRarity()
        {
            int Cursor = 0;
            var ListByChance = roles.Select(x => (x.Upper - x.Lower, x.Role)).OrderBy(x => x.Role == null ? int.MaxValue : x.Item1).ToList();

            List<(int Lower, int Upper, CustomRoleBaseInfo? Role)> NewRoles = new();

            foreach ((int Size, CustomRoleBaseInfo? Role) in ListByChance)
            {
                NewRoles.Add((Cursor, Cursor + Size, Role));
                Cursor += Size;
            }

            roles = NewRoles;
        }

        public CustomRoleBaseInfo? GetRandomRole()
        {
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
                CL.Error("roles count was zero");
                return null;
            }

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
                CL.Debug($"got -1 rolepos. {adjustedNumber}");
                return null;
            }


            if (!ValidateRole(rolePos))
            {
                RemoveRole(rolePos);
                return GetRandomRole(unadjustedRandom);
            }


            var role = RemoveRole(rolePos);


            return role;
        }

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
            var role = roles[RolePos];

            if (role.Role == null)
                return null;
            int middleChance = role.Upper - role.Lower;
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
