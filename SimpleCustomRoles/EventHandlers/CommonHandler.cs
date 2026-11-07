using LabApi.Events.CustomHandlers;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a common event handler.
/// </summary>
internal sealed class CommonHandler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnServerWaitingForPlayers()
	{
		CustomRoles.LoadRoles();
		CL.Info("Loaded custom roles!");
	}
}
