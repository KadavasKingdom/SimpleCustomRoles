namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for interacting with SCP-330.
/// </summary>
public interface ITakeCandyRole
{
	/// <summary>
	/// Gets a value indicating whether the role can take candy from SCP-330.
	/// </summary>
	public bool TakeCandyEnabled { get; }

	/// <summary>
	/// Gets the maximum candies to take from SCP-330.
	/// </summary>
	public int TakeCandyMax { get; }
}
