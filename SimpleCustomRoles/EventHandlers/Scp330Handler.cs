using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a scp-330 event handler.
/// </summary>
internal sealed class Scp330Handler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnPlayerInteractingScp330(PlayerInteractingScp330EventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not ITakeCandyRole takeCandyRole)
		{
			return;
		}

		if (ev.IsAllowed)
		{
			ev.IsAllowed = takeCandyRole.TakeCandyEnabled;
			if (!ev.IsAllowed)
			{
				return;
			}
		}

		ev.AllowPunishment = ev.Uses >= takeCandyRole.TakeCandyMax;
	}
}
