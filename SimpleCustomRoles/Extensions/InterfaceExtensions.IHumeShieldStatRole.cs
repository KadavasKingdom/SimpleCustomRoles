using PlayerRoles.PlayableScps.HumeShield;
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
	public static void Apply([NotNull] this IHumeShieldStatRole role, [NotNull] Player player)
	{
		float newValue = role.HumeShieldRegenRate.MathCalculation(player.HumeShieldRegenRate);
		if (newValue != player.HumeShieldRegenRate)
		{
			player.HumeShieldRegenRate = newValue;
		}

		newValue = role.HumeShieldRegenCooldown.MathCalculation(player.HumeShieldRegenCooldown);
		if (newValue != player.HumeShieldRegenCooldown)
		{
			player.HumeShieldRegenCooldown = newValue;
		}

		if (player.RoleBase is not IHumeShieldedRole humeShieldedRole)
		{
			return;
		}

		if (humeShieldedRole.HumeShieldModule is not DynamicHumeShieldController dynamicShield)
		{
			return;
		}

		if (role.ClearShieldOverHealth)
		{
			dynamicShield.ShieldOverHealth.ClearKeys();
		}

		foreach (KeyValuePair<float, float> item in role.ShieldOverHealth)
		{
			dynamicShield.ShieldOverHealth.AddKey(item.Key, item.Value);
		}
	}
}
