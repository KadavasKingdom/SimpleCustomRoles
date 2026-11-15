using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a use event handler.
/// </summary>
internal sealed class UseHandler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IUsableItemRole usableItemRole)
		{
			return;
		}

		if (ev.UsableItem is Scp330Item item && item.Base.IsCandySelected)
		{
			CandyKindID candyKind = item.Candies[item.SelectedCandyIndex];
			if (usableItemRole.CannotUserCandies.Contains(candyKind))
			{
				ev.IsAllowed = false;
				return;
			}
		}

		if (usableItemRole.CannotUserItems.Contains(ev.UsableItem.Type))
		{
			ev.IsAllowed = false;
			return;
		}
	}

	/// <inheritdoc/>
	public override void OnPlayerShootingWeapon(PlayerShootingWeaponEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IUsableItemRole usableItemRole)
		{
			return;
		}

		if (usableItemRole.CannotUserItems.Contains(ev.FirearmItem.Type))
		{
			ev.IsAllowed = false;
			return;
		}
	}
}
