using PlayerRoles;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface represent when the role spawn in the wave.
/// </summary>
public interface IWaveSpawnableRole
{
	/// <summary>
	/// Gets the faction of the wave to spawn the role with.
	/// </summary>
	public Faction WaveFaction { get; }

	/// <summary>
	/// Gets the minimum player count this role can spawn with the wave.
	/// </summary>
	/// <remarks>
	/// Value as <see cref="uint.MinValue"/> means there are not minimum set.
	/// </remarks>
	public uint MinWavePlayerCount { get; }

	/// <summary>
	/// Gets how many times the role can spawn with the wave.
	/// </summary>
	public uint WaveSpawnCount { get; }
}
