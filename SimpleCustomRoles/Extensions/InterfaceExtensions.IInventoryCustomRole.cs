using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using SimpleCustomRoles.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Extensions;

/// <summary>
/// Provides extension for Custom Roles Interfaces.
/// </summary>
public static partial class InterfaceExtensions
{
	/// <summary>
	/// Applies <paramref name="role"/> to the <paramref name="player"/>.
	/// </summary>
	/// <param name="role">The role.</param>
	/// <param name="player">The player.</param>
	public static void Apply([NotNull] this IInventoryCustomRole role, [NotNull] Player player)
	{
		if (role.ClearInventory)
		{
			player.ClearInventory();
		}

		foreach (ItemType item in role.Items)
		{
			ItemBase itemBase = player.Inventory.ServerAddItem(item, ItemAddReason.StartingItem);
			if (itemBase is Firearm firearm && firearm.TryGetModule(out IPrimaryAmmoContainerModule ammo))
			{
				ammo.ServerModifyAmmo(ammo.AmmoMax);
			}
		}

		foreach (KeyValuePair<ItemType, ushort> ammo in role.Ammos)
		{
			player.SetAmmo(ammo.Key, ammo.Value);
		}

		if (role.Candies.Count != 0)
		{
			player.AddCandies(role.Candies);
		}
	}
}
