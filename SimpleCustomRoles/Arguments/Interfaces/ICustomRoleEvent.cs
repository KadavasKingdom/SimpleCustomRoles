using SimpleCustomRoles.Roles;

namespace SimpleCustomRoles.Arguments.Interfaces;

/// <summary>
/// Represents an event that involves a custom role.
/// </summary
public interface ICustomRoleEvent
{
	/// <summary>
	/// Gets the custom role.
	/// </summary>
	public CustomRoleBase? CustomRole { get; }
}
