using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Handler;

internal sealed class Scp173Handler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnScp173Teleporting(Scp173TeleportingEventArgs ev)
	{
		if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			ev.IsAllowed = role.Scp.Scp173.CanTeleport;
		}
	}

	/// <inheritdoc/>
	public override void OnScp173PlayingSound(Scp173PlayingSoundEventArgs ev)
	{
		if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			ev.IsAllowed = !role.Scp.Scp173.DisableSoundPlay.Contains(ev.SoundId);
		}
	}

	/// <inheritdoc/>
	public override void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
	{
		if (CustomRoleHelpers.TryGetCustomRole(ev.Target, out var role))
		{
			ev.IsAllowed = role.Extra.Observe173;
		}
	}

	/// <inheritdoc/>
	public override void OnScp173CreatingTantrum(Scp173CreatingTantrumEventArgs ev)
	{
		if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			ev.IsAllowed = role.Scp.Scp173.CanPlaceTantrum;
		}
	}

	/// <inheritdoc/>
	public override void OnScp173BreakneckSpeedChanging(Scp173BreakneckSpeedChangingEventArgs ev)
	{
		if (CustomRoleHelpers.TryGetCustomRole(ev.Player, out var role))
		{
			ev.IsAllowed = role.Scp.Scp173.CanUseBreakneckSpeed;
		}
	}
}
