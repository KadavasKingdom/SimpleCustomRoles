using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.Pools;

namespace SimpleCustomRoles.Handler;

public class Scp049Handler : CustomEventsHandler
{
    public override void OnScp049Attacking(Scp049AttackingEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role) && role != null)
            ev.CooldownTime = role.Scp.Scp049.AttackCooldownTime.MathCalculation(ev.CooldownTime);
    }

    public override void OnScp049ResurrectingBody(Scp049ResurrectingBodyEventArgs ev)
    {
        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
        {
            // Why is this called CanRecall? This is for disabling zombie resurrection 
            if (!role.Scp.Scp049.CanRecall)
            {
                ev.IsAllowed = false;
                return;
            }
            if (CustomRoleHelpers.SetNewRole(ev.Target, role.Scp.Scp049))
                return;
        }

        var newRole = PoolManager.ZombieReplacePool.GetRandomRole();
        if (newRole is not null)
        {
            Timing.CallDelayed(0.2f, () =>
            {
                CustomRoleHelpers.SetCustomInfoToPlayer(ev.Target, newRole);
            });
        }
    }

    public override void OnScp049StartingResurrection(Scp049StartingResurrectionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
            return;
        if (role.Extra.CannotRevivedByScp049)
            ev.IsAllowed = false;
    }
}
