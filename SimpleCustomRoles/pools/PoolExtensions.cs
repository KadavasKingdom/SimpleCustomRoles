using LabApi.Features.Wrappers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Pools
{
    public static class PoolExtensions
    {
        public static CustomRoleBaseInfo? GetRandomCustomRoleBaseInfo(this Player player) => PoolManager.GetRandomRole(player);
    }
}
