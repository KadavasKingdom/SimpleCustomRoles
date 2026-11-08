using PlayerRoles.PlayableScps.HumeShield;
using SimpleCustomRoles.Interfaces;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

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
	public static void Apply([NotNull] this IScaleRole role, [NotNull] Player player)
	{
		if (role.Scale != Vector3.one)
		{
			ScaleExtension.SetScale(player, role.Scale);
		}

		if (role.FakeScale != Vector3.one)
		{
			ScaleExtension.SetScale(player, role.FakeScale, true);
		}
	}
}
