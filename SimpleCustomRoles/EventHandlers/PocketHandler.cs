using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a pocket dimension event handler.
/// </summary>
internal sealed class PocketHandler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnPlayerEnteringPocketDimension(PlayerEnteringPocketDimensionEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IPocketDimensionRole pocketDimensionRole)
		{
			return;
		}

		ev.IsAllowed = pocketDimensionRole.PocketCanEnter;
	}

	/// <inheritdoc/>
	public override void OnPlayerLeavingPocketDimension(PlayerLeavingPocketDimensionEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IPocketDimensionRole pocketDimensionRole)
		{
			return;
		}

		ev.IsAllowed = pocketDimensionRole.PocketCanLeave;

		if (pocketDimensionRole.PocketForceFail && pocketDimensionRole.PocketForceSuccess)
		{
			CL.Warn($"Role ({customRoleBase.RoleName}) Pocket force settings is invalid!");
			return;
		}

		if (pocketDimensionRole.PocketForceFail)
		{
			ev.IsSuccessful = false;
			return;
		}

		if (pocketDimensionRole.PocketForceSuccess)
		{
			ev.IsSuccessful = true;
		}
	}
}
