using PlayerRoles;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.Roles;

/// <summary>
/// Represent an example role.
/// </summary>
internal sealed class ExampleRole : CustomRoleBase, IDisplayRole
{
	static ExampleRole()
	{
		CustomRoles.TypeMapping.Add(typeof(ExampleRole).FullName, typeof(ExampleRole));
	}

	/// <inheritdoc/>
	public override string ParserClassName => typeof(ExampleRole).FullName;

	/// <inheritdoc/>
	public override string RoleName => "Example";

	/// <inheritdoc/>
	public override RoleTypeId RoleToSpawn => RoleTypeId.FacilityGuard;

	/// <inheritdoc/>
	public override RoleTypeId ReplaceRole => RoleTypeId.None;

	/// <inheritdoc/>
	public override Team ReplaceTeam => Team.ClassD;

	/// <inheritdoc/>
	public bool CanDisplay { get; set; }

	/// <inheritdoc/>
	public string NameDisplay => "Example";

	/// <inheritdoc/>
	public string NameRA => "RAExample";

	/// <inheritdoc/>
	public string NameSpectator => string.Empty;

	/// <inheritdoc/>
	public string ColorHex => "#FFFFFF";
}
