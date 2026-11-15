using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using UnityEngine;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a pocket dimension scaling event handler.
/// </summary>
internal sealed class PocketScaleHandler : CustomEventsHandler
{
	private static readonly Dictionary<string, Vector3> PlayerToScale = [];

	/// <inheritdoc/>
	public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev)
	{
		PlayerToScale[ev.Player.UserId] = ev.Player.Scale;
		ScaleExtension.SetScale(ev.Player, Vector3.one);
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
