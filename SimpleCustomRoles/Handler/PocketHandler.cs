using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
using UnityEngine;

namespace SimpleCustomRoles.Handler;

public class PocketHandler : CustomEventsHandler
{
    private static readonly Dictionary<Player, Vector3> PlayerToScale = [];

    public override void OnPlayerEnteringPocketDimension(PlayerEnteringPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Pocket.CanEnter;
    }

    public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev)
    {
        PlayerToScale[ev.Player] = ev.Player.Scale;
        ScaleHelper.SetScale(ev.Player, Vector3.one);
    }

    public override void OnPlayerLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
            return;
        ev.IsAllowed = role.Pocket.CanExit;
        if (ev.IsAllowed)
            ev.IsSuccessful = role.Pocket.ForceExit;
    }

    public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
    {
        if (!ev.IsSuccessful)
            return;
        if (PlayerToScale.TryGetValue(ev.Player, out Vector3 scale))
        {
            ScaleHelper.SetScale(ev.Player, scale);
            PlayerToScale.Remove(ev.Player);
        }
    }
}
