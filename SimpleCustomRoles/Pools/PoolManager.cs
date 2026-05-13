using LabApi.Features.Wrappers;
using PlayerRoles;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using UnityEngine;

namespace SimpleCustomRoles.Pools
{
    public static class PoolManager
    {
        internal static Dictionary<RoleTypeId, Pool> RoleReplacePools = new();

        internal static Dictionary<Team, Pool> TeamReplacePools = new();

        internal static Pool ZombieReplacePool = new();

        internal static List<CustomRoleBaseInfo> AlreadySpawnedRoles = new();

        // Resets all role pools (or only wave pools if OnlyWaves is true)
        public static void Reset(bool OnlyWaves = false)
        {
            // These can be recreated regardless of OnlyWaves (since theyre only used at the start of the round)
            RoleReplacePools = new();
            TeamReplacePools = new();
            _RoleGetterCache = new();

            if (!OnlyWaves)
            {
                // p.s. why does clearing a list zero it out ??
                AlreadySpawnedRoles.Clear();
                ZombieReplacePool = new(RoleTypeId.Scp0492);
            }

            // Add all roles to their respective pools
            foreach (var customRole in RolesLoader.RoleInfos)
            {
                if (OnlyWaves && customRole.RoleType != RoleYaml.Enums.CustomRoleType.InWave)
                    continue;


                if (customRole.ReplaceRole != RoleTypeId.None)
                {
                    var pool = RoleReplacePools.GetOrAdd(customRole.ReplaceRole, () => new(customRole.ReplaceRole));

                    pool.AddRole(customRole);
                }
                else if (customRole.ReplaceTeam != Team.Dead)
                {
                    var pool = TeamReplacePools.GetOrAdd(customRole.ReplaceTeam, () => new());

                    pool.AddRole(customRole);
                }
            }

            // Sort all pools, and add adjustmentFunc for spawn rate bias (https://www.desmos.com/calculator/0cobiudqi9)
            foreach (var pool in RoleReplacePools.Values.Concat(TeamReplacePools.Values))
            {
                // Add empty role, for chance of getting no custom role
                pool.AddRole(null);
                pool.SortByRarity();
                pool.AddAdjustmentFunc(x => Mathf.Pow(x, Mathf.Pow(1.2f, Main.Instance.Config.SpawnRateBias - 1)));
            }

            // Set zombie pool (or leave it as an empty pool)
            var ZombiePool = RoleReplacePools.Where(x => x.Key == RoleTypeId.Scp0492);
            ZombieReplacePool = ZombiePool.FirstOrDefault().Value ?? ZombieReplacePool;
        }

        public static CustomRoleBaseInfo? GetRandomRole(Player player) => RoleReplacePools!.Get(player.Role)?.GetRandomRole();

        // Cache for when the exact same predicate is used for the exact same role type
        static Dictionary<(RoleTypeId, Predicate<CustomRoleBaseInfo>), Func<CustomRoleBaseInfo?>> _RoleGetterCache = new();

        // use the same predicate object if the predicate is logically the same (so it can be cached)
        // Returns a function that can be called to get a random role from a pool in which roles which predicate returns false for are removed.
        internal static Func<CustomRoleBaseInfo?> GetRandomRoleGetterPredicate(RoleTypeId role, Predicate<CustomRoleBaseInfo> predicate)
        {
            if (_RoleGetterCache.TryGetValue((role, predicate), out var cached))
                return cached;

            

            var pool = RoleReplacePools!.Get(role);


            // if pool is null, all rolls will just return null.
            if (pool == null)
                return () => null;

            var tempPool = new Pool(role);
            tempPool.AdjustmentFuncs = pool.AdjustmentFuncs;

            // Copy pool roles to new temporary pool
            foreach ((int Lower, int Upper, var Role) in pool.roles)
            {
                if (Role == null || predicate(Role))
                    tempPool.AddRole(Role, Upper - Lower, 1);
            }

            

            var func = () => tempPool.GetRandomRole();

            _RoleGetterCache.Add((role, predicate), func);

            return func;
        }
    }
}
