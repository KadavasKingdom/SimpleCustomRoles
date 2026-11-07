using PlayerRoles;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for changing the appearance of the role.
/// </summary>
public interface IAppearanceRole
{
	/// <summary>
	/// Gets the role apperance for others.
	/// </summary>
	public RoleTypeId Appearance { get; }
}
