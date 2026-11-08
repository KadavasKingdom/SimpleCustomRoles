using UnityEngine;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for spawning on the position.
/// </summary>
public interface ISpawnPositionRole
{
	/// <summary>
	/// Gets the position to spawn.
	/// </summary>
	public Vector3 SpawnPosition { get; }
}
