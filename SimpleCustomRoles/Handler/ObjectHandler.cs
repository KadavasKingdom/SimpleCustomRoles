using LabApi.Events.Arguments.ObjectiveEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Handler;

internal class ObjectHandler : CustomEventsHandler
{
    public override void OnObjectiveDamagingScpCompleting(ScpDamagingObjectiveEventArgs ev)
    {
        CustomRoleBaseInfo role = null;

        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out role) && role != null)
        {
            ev.TimeToGrant = role.Ticket.TimeGrantDamage.MathCalculation(ev.TimeToGrant);
            ev.InfluenceToGrant = role.Ticket.InfluenceGrantDamage.MathCalculation(ev.InfluenceToGrant);
        }

        if (CustomRoleHelpers.TryGetCustomRole(ev.Target, out role) && role != null)
        {
            ev.TimeToGrant = role.Ticket.TimeGrantDamage.MathCalculation(ev.TimeToGrant);
            ev.InfluenceToGrant = role.Ticket.InfluenceGrantDamage.MathCalculation(ev.InfluenceToGrant);
        }
            
    }

    public override void OnObjectiveKillingEnemyCompleting(EnemyKillingObjectiveEventArgs ev)
    {
        CustomRoleBaseInfo role = null;

        if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out role) && role != null)
        {
            ev.TimeToGrant = role.Ticket.TimeGrantKill.MathCalculation(ev.TimeToGrant);
            ev.InfluenceToGrant = role.Ticket.InfluenceGrantKill.MathCalculation(ev.InfluenceToGrant);
        }

        if (CustomRoleHelpers.TryGetCustomRole(ev.Target, out role) && role != null)
        {
            ev.TimeToGrant = role.Ticket.TimeGrantKill.MathCalculation(ev.TimeToGrant);
            ev.InfluenceToGrant = role.Ticket.InfluenceGrantKill.MathCalculation(ev.InfluenceToGrant);
        }
    }
}
