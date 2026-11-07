using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using SimpleCustomRoles.Interfaces;
using UnityEngine;

namespace SimpleCustomRoles.Roles;

/// <summary>
/// Represent a simple custom role.
/// </summary>
public sealed class SimpleRole : CustomRoleBase, IDisplayRole, IAllStatRole, IEffectRole, IAppearanceRole, IScaleRole, IGravityRole, IInventoryCustomRole, ICustomInventoryRole
{
	/// <inheritdoc/>
	public bool CanDisplay { get; set; }

	/// <inheritdoc/>
	public string NameDisplay { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string NameRA { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string NameSpectator { get; set; } = string.Empty;

	/// <inheritdoc/>
	public string ColorHex { get; set; } = string.Empty;

	/// <inheritdoc/>
	public MathValueFloat Ahp { get; set; } = new();

	/// <inheritdoc/>
	public MathValueFloat MaxAhp { get; set; } = new();

	/// <inheritdoc/>
	public MathValueFloat Health { get; set; } = new();

	/// <inheritdoc/>
	public MathValueFloat MaxHealth { get; set; } = new();

	/// <inheritdoc/>
	public MathValueFloat HumeShieldRegenRate { get; set; } = new();

	/// <inheritdoc/>
	public MathValueFloat HumeShieldRegenCooldown { get; set; } = new();

	/// <inheritdoc/>
	public Dictionary<float, float> ShieldOverHealth { get; set; } = [];

	/// <inheritdoc/>
	public bool ClearShieldOverHealth { get; set; }

	/// <inheritdoc/>
	public MathValueFloat MaxStamina { get; set; } = new();

	/// <inheritdoc/>
	public List<EffectData> Effects { get; set; } = [];

	/// <inheritdoc/>
	public RoleTypeId Appearance { get; set; } = RoleTypeId.None;

	/// <inheritdoc/>
	public Vector3 Scale { get; set; } = Vector3.one;

	/// <inheritdoc/>
	public Vector3 FakeScale { get; set; } = Vector3.one;

	/// <inheritdoc/>
	public Vector3 Gravity { get; set; } = FpcGravityController.DefaultGravity;

	/// <inheritdoc/>
	public bool Clear { get; set; }

	/// <inheritdoc/>
	public List<ItemType> Items { get; set; } = [];

	/// <inheritdoc/>
	public Dictionary<ItemType, ushort> Ammos { get; set; } = [];

	/// <inheritdoc/>
	public List<uint> CustomIds { get; set; } = [];

	/// <inheritdoc/>
	public List<string> CustomNames { get; set; } = [];
}
