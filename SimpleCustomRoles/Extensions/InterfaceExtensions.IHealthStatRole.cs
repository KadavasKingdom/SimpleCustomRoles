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
	public static void Apply([NotNull] this IHealthStatRole role, [NotNull] Player player)
	{
		float newValue = role.MaxHealth.MathCalculation(player.MaxHealth);
		if (newValue != player.MaxHealth)
		{
			player.MaxHealth = newValue;
		}

		newValue = role.Health.MathCalculation(player.Health);
		if (newValue != player.Health)
		{
			player.Health = newValue;
		}
	}
}
