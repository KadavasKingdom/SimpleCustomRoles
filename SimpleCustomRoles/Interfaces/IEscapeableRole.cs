namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for role to be escapeable.
/// </summary>
public interface IEscapeableRole
{
	/// <summary>
	/// Gets a value indicating whether the role can escaoe.
	/// </summary>
	public bool CanEscape { get; }
}
