namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for interacting with pocket dimension.
/// </summary>
public interface IPocketDimensionRole
{
	/// <summary>
	/// Gets a value indicating whether the role can enter the pocket dimension.
	/// </summary>
	public bool PocketCanEnter { get; }

	/// <summary>
	/// Gets a value indicating whether the role can leave the pocket dimension.
	/// </summary>
	public bool PocketCanLeave { get; }

	/// <summary>
	/// Gets a value indicating whether the role leaving the pocket dimension force success exit.
	/// </summary>
	public bool PocketForceSuccess { get; }

	/// <summary>
	/// Gets a value indicating whether the role leaving the pocket dimension force fail to exit.
	/// </summary>
	public bool PocketForceFail { get; }
}
