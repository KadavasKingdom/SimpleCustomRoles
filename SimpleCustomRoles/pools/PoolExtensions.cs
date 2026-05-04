using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleYaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCustomRoles.Pools
{
    public static class PoolExtensions
    {
        public static CustomRoleBaseInfo? GetRandomCustomRoleBaseInfo(this Player player) => PoolManager.GetRandomRole(player);
    }
}
