using InventorySystem.Items.Usables.Scp330;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for rols that cannot use specific items or candies.
/// </summary>
public interface IUsableItemRole
{
	/// <summary>
	/// Gets the list of items that the role cannot use.
	/// </summary>
	public List<ItemType> CannotUseItems { get; }

	/// <summary>
	/// Gets the list of candies that the role cannot use.
	/// </summary>
	public List<CandyKindID> CannotUseCandies { get; }
}
