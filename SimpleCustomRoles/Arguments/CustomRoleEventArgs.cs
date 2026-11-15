using SimpleCustomRoles.Arguments.Interfaces;
using SimpleCustomRoles.Roles;

namespace SimpleCustomRoles.Arguments;

/// <summary>
/// Represents the arguments for the common custom role events.
/// </summary>
/// <param name="customRole">The custom role.</param>
public class CustomRoleEventArgs(CustomRoleBase customRole) : EventArgs, ICustomRoleEvent
{
	/// <inheritdoc/>
	public CustomRoleBase CustomRole { get; } = customRole;
}
