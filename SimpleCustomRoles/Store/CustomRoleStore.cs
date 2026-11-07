using LabApi.Features.Stores;
using SimpleCustomRoles.Roles;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Store;

/// <summary>
/// Represent a custom role store.
/// </summary>
/// <param name="player">The owner.</param>
public class CustomRoleStore(Player player) : CustomDataStore(player)
{
	/// <summary>
	/// Gets the custom role.
	/// </summary>
	protected CustomRoleBase? MyCustomRole { get; private set; }

	/// <summary>
	/// Tries to get the custom role.
	/// </summary>
	/// <param name="role">The role.</param>
	/// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
	public bool TryGetCustomRole([NotNullWhen(true)] out CustomRoleBase? role)
	{
		role = MyCustomRole;
		return role != null;
	}

	/// <summary>
	/// Set the custom role to <paramref name="role"/>.
	/// </summary>
	/// <param name="role">The role.</param>
	public void SetCustomRole(CustomRoleBase? role)
	{
		MyCustomRole = role;
	}
}
