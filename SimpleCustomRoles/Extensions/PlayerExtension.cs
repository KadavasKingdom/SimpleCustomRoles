using InventorySystem.Items;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles.FirstPersonControl;

namespace SimpleCustomRoles.Extensions;

/// <summary>
/// Extesnion for <see cref="Player"/>.
/// </summary>
public static class PlayerExtension
{
	/// <summary>
	/// Get the height of the player.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <returns>The character controller height or <see cref="float.NaN"/>.</returns>
	public static float GetPlayerHeight(this Player player)
	{
		if (player == null)
		{
			return float.NaN;
		}

		if (player.ReferenceHub.roleManager.CurrentRole is not IFpcRole fpcRole)
		{
			return float.NaN;
		}

		return fpcRole.FpcModule.CharController.height;
	}

	/// <summary>
	/// Adjust <paramref name="position"/> for the <paramref name="player"/>.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="position">The position to adjust.</param>
	public static void AdjustForPlayer(this Player player, ref UnityEngine.Vector3 position)
	{
		float height = player.GetPlayerHeight();
		if (float.IsNaN(height))
		{
			return;
		}

		position.y += height;
		return;
	}

	/// <summary>
	/// Add candies to <paramref name="player"/>.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="candies">Any number of candies.</param>
	/// <remarks>
	/// Support adding more than <see cref="Scp330Bag.MaxCandies"/>.
	/// </remarks>
	public static void AddCandies(this Player player, List<CandyKindID> candies)
	{
		if (candies == null)
		{
			return;
		}

		int candiesCount = candies.Count;

		for (int i = 0; i < candiesCount; i += 8)
		{
			Scp330Item? item = TryGetNonFullCandyBag(player);
			if (item == null)
			{
				return;
			}

			var splittedCandies = candies.Skip(i).Take(8);
			item.SetCandies(splittedCandies);
		}
	}

	/// <summary>
	/// Tries to get or adds a candy bag from <paramref name="player"/> that is not full.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <returns>The found or new candy bag or <see langword="null"/>.</returns>
	public static Scp330Item? TryGetNonFullCandyBag(this Player player)
	{
		if (player == null)
		{
			return null;
		}

		foreach (Item item in player.Items)
		{
			if (item is not Scp330Item scp330Item)
			{
				continue;
			}

			if (scp330Item.Candies.Count == Scp330Bag.MaxCandies)
			{
				continue;
			}

			return scp330Item;
		}

		if (!player.IsInventoryFull)
		{
			return (Scp330Item?)player.AddItem(ItemType.SCP330, ItemAddReason.StartingItem);
		}

		return null;
	}
}
