using CustomPlayerEffects;
using MEC;
using SimpleCustomRoles.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Extensions;

#pragma warning disable CA1034 // Nested types should not be visible

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
	public static void Apply([NotNull] this IEffectRole role, [NotNull] Player player)
	{
		// Time needed for applying effect.
		float time = 0.2f;
		if (SpawnProtected.IsProtectionEnabled && SpawnProtected.ProtectedTeams.Contains(player.Team))
		{
			time += SpawnProtected.SpawnDuration;
		}

		foreach (var effect in role.Effects)
		{
			if (!effect.Immune)
			{
				continue;
			}

			Timing.CallDelayed(time, () => player.EnableEffect(effect.EffectName, effect.Intensity, effect.Duration));
		}
	}
}
