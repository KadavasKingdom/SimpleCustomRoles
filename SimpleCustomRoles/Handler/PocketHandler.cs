using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
using UnityEngine;

namespace SimpleCustomRoles.Handler;

internal sealed class PocketHandler : CustomEventsHandler
{
	private static readonly Dictionary<string, Vector3> PlayerToScale = [];

	/// <inheritdoc/>
	public override void OnPlayerEnteringPocketDimension(PlayerEnteringPocketDimensionEventArgs ev)
	{
		if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			return;
		}

		ev.IsAllowed = role.Pocket.CanEnter;
	}

	/// <inheritdoc/>
	public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev)
	{
		PlayerToScale[ev.Player.UserId] = ev.Player.Scale;
		ScaleExtension.SetScale(ev.Player, Vector3.one);
	}

	/// <inheritdoc/>
	public override void OnPlayerLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
	{
		if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			return;
		}

		ev.IsAllowed = role.Pocket.CanExit;
		if (ev.IsAllowed)
		{
			ev.IsSuccessful = role.Pocket.ForceExit;
		}
	}

	/// <inheritdoc/>
	public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
	{
		if (!ev.IsSuccessful)
		{
			return;
		}

		if (PlayerToScale.TryGetValue(ev.Player.UserId, out Vector3 scale))
		{
			ScaleExtension.SetScale(ev.Player, scale);
			PlayerToScale.Remove(ev.Player.UserId);
		}
	}
}
