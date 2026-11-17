using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerRoles;
using SimpleCustomRoles.Datas;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a drop event handler.
/// </summary>
internal sealed class EscapeHandler : CustomEventsHandler
{
	// TODO: Cahce for roles/player that cannot escape.
	private static void EscapeNonCustomRole(ref PlayerEscapingEventArgs ev)
	{
		bool isCuffed = ev.Player.IsDisarmed;
		RoleTypeId currentRole = ev.OldRole;
		EscapeData data = new(isCuffed, currentRole);

		RoleTypeId newRole = Main.Instance.Config.EscapeRoleChange.Get(data, RoleTypeId.None);

		if (newRole is not (RoleTypeId.None or RoleTypeId.Destroyed))
		{
			ev.IsAllowed = true;
			ev.NewRole = newRole;
			ev.EscapeScenario = Escape.EscapeScenarioType.Custom;
			return;
		}
	}

	private static void EscapeCustomRole(IEscapeableRole escapeableRole, ref PlayerEscapingEventArgs ev)
	{
	}

	/// <inheritdoc/>
	public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			EscapeNonCustomRole(ref ev);
			return;
		}

		if (customRoleBase is not IEscapeableRole escapeableRole)
		{
			return;
		}

		EscapeCustomRole(escapeableRole, ref ev);
	}
}
