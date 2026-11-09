using LabApi.Features.Stores;
using SimpleCustomRoles.Roles;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Store;

/// <summary>
/// Represent a custom role store.
/// </summary>
/// <param name="player">The owner.</param>
public sealed class CustomRoleStore(Player player) : CustomDataStore(player)
{
	/// <summary>
	/// Gets or sets a value indicating whether should reset the the role.
	/// </summary>
	public bool ShouldResetRole { get; set; }

	/// <summary>
	/// Gets or sets the original info area.
	/// </summary>
	public PlayerInfoArea OriginalInfoAraea { get; set; }

	/// <summary>
	/// Gets or sets the original custom info.
	/// </summary>
	public string OriginalCustomInfo { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the custom role.
	/// </summary>
	private CustomRoleBase? MyCustomRole { get; set; }

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
