using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a drop event handler.
/// </summary>
internal sealed class EscapeHandler : CustomEventsHandler
{
	// TODO: Cahce for roles/player that cannot escape.

	/// <inheritdoc/>
	public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
	{
		return;

		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IEscapeableRole escapeableRole)
		{
			return;
		}

		ev.IsAllowed = escapeableRole.CanEscape;
	}
}
