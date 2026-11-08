using MapGeneration;
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
	public static void Apply([NotNull] this ISpawnRandomLocationRole role, [NotNull] Player player)
	{
		if (!role.SpawnRandomLocation)
		{
			return;
		}

		Vector3 offset = role is ISpawnOffsetLocationRole spawnOffset ? spawnOffset.SpawnOffset : Vector3.zero;

		Room? room = Room.List.GetRandom();
		if (room == null)
		{
			return;
		}

		Vector3 spawnPos = room.Position + offset;
		player.AdjustForPlayer(ref spawnPos);

		player.Position = spawnPos;
	}
}
