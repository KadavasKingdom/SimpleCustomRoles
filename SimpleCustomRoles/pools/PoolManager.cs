using LabApi.Features.Wrappers;
using PlayerRoles;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleCustomRoles.Pools
{
    public static class PoolManager
    {
        internal static Dictionary<RoleTypeId, Pool> RoleReplacePools = new();

        internal static Dictionary<Team, Pool> TeamReplacePools = new();

        internal static List<CustomRoleBaseInfo> AlreadySpawnedRoles = new();

        public static void Reset(bool OnlyWaves = false)
        {
            RoleReplacePools = new();
            TeamReplacePools = new();
            if (!OnlyWaves)
                // p.s. why does clearing a list zero it out ??
                AlreadySpawnedRoles.Clear();

            _RoleGetterCache = new();


            foreach (var customRole in RolesLoader.RoleInfos)
            {
                if (OnlyWaves && customRole.RoleType != RoleYaml.Enums.CustomRoleType.InWave)
                    continue;


                if (customRole.ReplaceRole != RoleTypeId.None)
                {
                    var pool = RoleReplacePools.GetOrAdd(customRole.ReplaceRole, () => new());

                    pool.AddRole(customRole);
                } else if (customRole.ReplaceTeam != Team.Dead)
                {
                    var pool = TeamReplacePools.GetOrAdd(customRole.ReplaceTeam, () => new());

                    pool.AddRole(customRole);
                }
            }

            foreach ((RoleTypeId RoleType, Pool Pool) in RoleReplacePools.Select(x => (x.Key, x.Value)))
            {
                CL.Info($"{RoleType}");
                Pool.Print();
            }

            foreach (var pool in RoleReplacePools.Values.Concat(TeamReplacePools.Values))
            {
                pool.SortByRarity();

                CL.Info($"{Main.Instance.Config.SpawnRateMultiplier}, {Mathf.Log(Main.Instance.Config.SpawnRateMultiplier * 2, 2)}");
                pool.AddAdjustmentFunc(num => Mathf.Pow(num, Mathf.Log(Main.Instance.Config.SpawnRateMultiplier * 2, 2)));
            }
        }

        public static CustomRoleBaseInfo? GetRandomRole(Player player) => RoleReplacePools!.Get(player.Role)?.GetRandomRole();

        static Dictionary<(RoleTypeId, Predicate<CustomRoleBaseInfo>), Func<CustomRoleBaseInfo?>> _RoleGetterCache = new();

        /// <summary>use the same predicate object if the predicate is logically the same (so it can be cached)</summary>
        internal static Func<CustomRoleBaseInfo?> GetRandomRoleGetterPredicate(RoleTypeId role, Predicate<CustomRoleBaseInfo> predicate)
        {
            if (_RoleGetterCache.TryGetValue((role, predicate), out var cached))
            {
                return cached;
            }

            var tempPool = new Pool();
            var pool = RoleReplacePools!.Get(role);
            if (pool == null)
            {
                return () => null;
            }

            foreach ((_, _, var Role) in pool.roles)
            {
                if (predicate(Role))
                {
                    tempPool.AddRole(Role, false);
                }
            }

            tempPool.AdjustmentFuncs = pool.AdjustmentFuncs;

            var func = () => tempPool.GetRandomRole();

            _RoleGetterCache.Add((role, predicate), func);

            return func;
        }
    }
}
