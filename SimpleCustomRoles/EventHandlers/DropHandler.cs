using InventorySystem.Items.Usables.Scp330;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a drop event handler.
/// </summary>
internal sealed class DropHandler : CustomEventsHandler
{
	/// <inheritdoc/>
	public override void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev)
	{
		if (!CustomRoles.TryGetCustomRole(ev.Player, out var customRoleBase))
		{
			return;
		}

		if (customRoleBase is not IDroppableRole droppableRole)
		{
			return;
		}

		if (ev.Item is Scp330Item item && item.Base.IsCandySelected)
		{
			CandyKindID candyKind = item.Candies[item.SelectedCandyIndex];
			if (droppableRole.CannotDropCandies.Contains(candyKind))
			{
				ev.IsAllowed = false;
				return;
			}
		}

		if (droppableRole.CannotDropItems.Contains(ev.Item.Type))
		{
			ev.IsAllowed = false;
			return;
		}
	}
}
