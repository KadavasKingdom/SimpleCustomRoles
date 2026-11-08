namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for spawning role in random location.
/// </summary>
public interface ISpawnRandomLocationRole
{
	/// <summary>
	/// Gets a value indicating whether to spawn in random location.
	/// </summary>
	public bool SpawnRandomLocation { get; }
}
