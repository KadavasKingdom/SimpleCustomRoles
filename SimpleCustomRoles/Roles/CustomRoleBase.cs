using PlayerRoles;
using SimpleCustomRoles.Attributes;
using YamlDotNet.Serialization;

namespace SimpleCustomRoles.Roles;

/// <summary>
/// Represent a custom role base.
/// </summary>
public abstract class CustomRoleBase
{
	/// <summary>
	/// Gets the parser class name.
	/// </summary>
	[YamlMember(Order = int.MinValue)]
	[YamlSerializeProperty]
	public virtual string ParserClassName { get; internal set; } = string.Empty;

	/// <summary>
	/// Gets or sets a value indicating whether the role is enabled.
	/// </summary>
	[YamlMember(Order = -100)]
	public virtual bool Enabled { get; set; } = true;

	/// <summary>
	/// Gets the role name.
	/// </summary>
	[YamlMember(Order = -99)]
	public virtual string RoleName { get; internal set; } = string.Empty;

	/// <summary>
	/// Gets the role type to spawn.
	/// </summary>
	[YamlMember(Order = -98)]
	public virtual RoleTypeId RoleToSpawn { get; internal set; }

	/// <summary>
	/// Gets the role type to replace.
	/// </summary>
	[YamlMember(Order = -97)]
	public virtual RoleTypeId ReplaceRole { get; internal set; }

	/// <summary>
	/// Gets the team to replace.
	/// </summary>
	/// <remarks>
	/// Use this if you wanna replace any SCP for example.
	/// </remarks>
	[YamlMember(Order = -96)]
	public virtual Team ReplaceTeam { get; internal set; } = Team.Dead;

	/// <summary>
	/// Called once when this instance is registered.
	/// </summary>
	public virtual void OnRegistered()
	{
	}
}
