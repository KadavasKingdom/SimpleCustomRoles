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
        internal static Dictionary<RoleTypeId, Pool> ReplacePools = new();

        public static void Reset(bool OnlyWaves = false)
        {
            ReplacePools = new();



            foreach (var customRole in RolesLoader.RoleInfos)
            {
                if (OnlyWaves && customRole.RoleType != RoleYaml.Enums.CustomRoleType.InWave)
                    continue;


                if (customRole.ReplaceRole != RoleTypeId.None)
                {
                    var pool = ReplacePools.GetOrAdd(customRole.ReplaceRole, () => new());

                    pool.AddRole(customRole);
                }
            }

            foreach ((RoleTypeId RoleType, Pool Pool) in ReplacePools.Select(x => (x.Key, x.Value)))
            {
                CL.Info($"{RoleType}");
                Pool.Print();
            }

            foreach (var pool in ReplacePools.Values)
            {
                pool.SortByRarity();

                pool.AddAdjustmentFunc(num => Mathf.Pow(num, Mathf.Log(Main.Instance.Config.SpawnRateMultiplier * 2, 2)));
            }
        }

        public static CustomRoleBaseInfo? GetRandomRole(Player player) => ReplacePools!.Get(player.Role)?.GetRandomRole();
    }
}
