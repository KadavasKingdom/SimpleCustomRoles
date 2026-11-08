namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for making the roles spawnable on round start.
/// </summary>
public interface ISpawnableRole
{
	/// <summary>
	/// Gets the chance the role can spawn.
	/// </summary>
	/// <remarks>
	/// Value as <see cref="ushort.MinValue"/> means the role never spawn.
	/// Spawning chance values between 0 and 10_000.
	/// 1% spawn chance are represented as 100.
	/// </remarks>
	public ushort SpawnChance { get; }

	/// <summary>
	/// Gets how many roles to spawn in the round.
	/// </summary>
	public uint SpawnCount { get; }

	/// <summary>
	/// Gets a value indicating whether the main config can edit the <see cref="SpawnChance"/>.
	/// </summary>
	public bool SpawnChanceEditable { get; }
}
