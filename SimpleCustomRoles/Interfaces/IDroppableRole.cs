using InventorySystem.Items.Usables.Scp330;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for rols that cannot drop specific items or candies.
/// </summary>
public interface IDroppableRole
{
	/// <summary>
	/// Gets the list of items that the role cannot drop.
	/// </summary>
	public List<ItemType> CannotDropItems { get; }

	/// <summary>
	/// Gets the list of candies that the role cannot drop.
	/// </summary>
	public List<CandyKindID> CannotDropCandies { get; }
}
