namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for making the roles spawnable on round start.
/// </summary>
public interface ISpawnableRole
{
	/// <summary>
	/// Gets how many roles to spawn in the round.
	/// </summary>
	public uint SpawnCount { get; }
}
