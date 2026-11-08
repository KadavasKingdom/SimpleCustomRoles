using InventorySystem.Items.Usables.Scp330;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for adding items into the role.
/// </summary>
public interface IInventoryCustomRole
{
	/// <summary>
	/// Gets a value indicating whether to clear the inventory.
	/// </summary>
	public bool ClearInventory { get; }

	/// <summary>
	/// Gets the items to add.
	/// </summary>
	public List<ItemType> Items { get; }

	/// <summary>
	/// Gets the ammos to add.
	/// </summary>
	public Dictionary<ItemType, ushort> Ammos { get; }

	/// <summary>
	/// Gets the candies to add.
	/// </summary>
	public List<CandyKindID> Candies { get; }
}
