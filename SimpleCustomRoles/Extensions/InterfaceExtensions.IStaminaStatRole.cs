using PlayerStatsSystem;
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
	public static void Apply([NotNull] this IStaminaStatRole role, [NotNull] Player player)
	{
		StaminaStat staminaStat = player.ReferenceHub.playerStats.GetModule<StaminaStat>();
		if (staminaStat == null)
		{
			return;
		}

		float newValue = role.MaxStamina.MathCalculation(staminaStat.MaxValue);
		if (newValue != staminaStat.MaxValue)
		{
			staminaStat.MaxValue = newValue;
		}
	}
}
