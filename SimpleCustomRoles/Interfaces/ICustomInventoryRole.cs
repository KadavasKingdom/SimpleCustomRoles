namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for adding custom items into the role.
/// </summary>
public interface ICustomInventoryRole
{
	/// <summary>
	/// Gets the items to add.
	/// </summary>
	public List<uint> CustomIds { get; }

	/// <summary>
	/// Gets the items to add.
	/// </summary>
	public List<string> CustomNames { get; }
}
