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
	public static void Apply([NotNull] this ISpawnPositionRole role, [NotNull] Player player)
	{
		if (role.SpawnPosition == Vector3.zero)
		{
			return;
		}

		player.Position = role.SpawnPosition;
	}
}
