namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for constrain for spawning the role.
/// </summary>
public interface ISpawnConstrainsRole
{
	/// <summary>
	/// Gets the minimum player count this role can spawn.
	/// </summary>
	/// <remarks>
	/// Value as <see cref="uint.MinValue"/> means there are not minimum set.
	/// </remarks>
	public uint MinPlayerCount { get; }

	/// <summary>
	/// Gets the maximum player count this role can spawn.
	/// </summary>
	/// <remarks>
	/// Value as <see cref="uint.MinValue"/> means there are not maximum set.
	/// </remarks>
	public uint MaxPlayerCount { get; }
}
