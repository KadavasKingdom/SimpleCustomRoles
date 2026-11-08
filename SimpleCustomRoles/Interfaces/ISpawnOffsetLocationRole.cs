using UnityEngine;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for spawning with the offset.
/// </summary>
/// <remarks>
/// Use it with <see cref="ISpawnRoomLocationRole"/> or <see cref="ISpawnZoneLocationRole"/>.
/// </remarks>
public interface ISpawnOffsetLocationRole
{
	/// <summary>
	/// Gets the offset to spawn.
	/// </summary>
	public Vector3 SpawnOffset { get; }
}
