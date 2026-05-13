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

        public static void Reset(bool OnlyWaves = false)
        {
            RoleReplacePools = new();
            TeamReplacePools = new();


            if (!OnlyWaves)
            {
                // p.s. why does clearing a list zero it out ??
                AlreadySpawnedRoles.Clear();
                ZombieReplacePool = new(RoleTypeId.Scp0492);
            }

            _RoleGetterCache = new();


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

            foreach (var pool in RoleReplacePools.Values.Concat(TeamReplacePools.Values))
            {
                pool.SortByRarity();
                pool.AddAdjustmentFunc(x => 1 - Mathf.Pow(x, Mathf.Pow(1.2f, Main.Instance.Config.SpawnRateBias - 1)));
            }

            foreach (var pool in RoleReplacePools.Values)
            {
                pool.AddRole(null);
            }

            var ZombiePool = RoleReplacePools.Where(x => x.Key == RoleTypeId.Scp0492);
            ZombieReplacePool = ZombiePool.FirstOrDefault().Value ?? ZombieReplacePool;
        }

        public static CustomRoleBaseInfo? GetRandomRole(Player player) => RoleReplacePools!.Get(player.Role)?.GetRandomRole();

        static Dictionary<(RoleTypeId, Predicate<CustomRoleBaseInfo>), Func<CustomRoleBaseInfo?>> _RoleGetterCache = new();

        // use the same predicate object if the predicate is logically the same (so it can be cached)
        internal static Func<CustomRoleBaseInfo?> GetRandomRoleGetterPredicate(RoleTypeId role, Predicate<CustomRoleBaseInfo> predicate)
        {
            if (_RoleGetterCache.TryGetValue((role, predicate), out var cached))
                return cached;

            var tempPool = new Pool();

            var pool = RoleReplacePools!.Get(role);
            if (pool == null)
                return () => null;

            foreach ((_, _, var Role) in pool.roles)
            {
                if (Role == null || predicate(Role))
                    tempPool.AddRole(Role, false);
            }

            tempPool.AdjustmentFuncs = pool.AdjustmentFuncs;

            var func = () => tempPool.GetRandomRole();

            _RoleGetterCache.Add((role, predicate), func);

            return func;
        }
    }
}
