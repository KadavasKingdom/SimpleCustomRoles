namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for role to be able to hide from spectators.
/// </summary>
public interface ISpectatorVisibilityRole
{
	/// <summary>
	/// Gets a value indicating whether to hide player from spectators.
	/// </summary>
	public bool IsHiddenFromSpectators { get; }
}
