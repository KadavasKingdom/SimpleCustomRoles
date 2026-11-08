using InventorySystem.Items.Usables.Scp330;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using SimpleCustomRoles.Extensions;
using SimpleCustomRoles.Interfaces;
using SimpleCustomRoles.Store;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace SimpleCustomRoles.Roles;

/// <summary>
/// Represent a simple custom role.
/// </summary>
public class SimpleRole : CustomRoleBase,
	IDisplayRole,
	IAllStatRole,
	IEffectRole,
	IAppearanceRole,
	IScaleRole,
	IGravityRole,
	IInventoryCustomRole,
	ICustomInventoryRole,
	ISpawnableRole,
	ISpawnConstrainsRole,
	IWaveSpawnableRole,
	ISpawnOffsetLocationRole,
	ISpawnPositionRole,
	ISpawnRoomLocationRole,
	ISpawnZoneLocationRole,
	ISpawnRandomLocationRole
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
	public bool ClearInventory { get; set; }

	/// <inheritdoc/>
	public List<ItemType> Items { get; set; } = [];

	/// <inheritdoc/>
	public Dictionary<ItemType, ushort> Ammos { get; set; } = [];

	/// <inheritdoc/>
	public List<uint> CustomIds { get; set; } = [];

	/// <inheritdoc/>
	public List<string> CustomNames { get; set; } = [];

	/// <inheritdoc/>
	public ushort SpawnChance { get; set; }

	/// <inheritdoc/>
	public uint SpawnCount { get; set; }

	/// <inheritdoc/>
	public bool SpawnChanceEditable { get; set; } = true;

	/// <inheritdoc/>
	public uint MinPlayerCount { get; set; }

	/// <inheritdoc/>
	public uint MaxPlayerCount { get; set; }

	/// <inheritdoc/>
	public Faction WaveFaction { get; set; } = Faction.Unclassified;

	/// <inheritdoc/>
	public uint MinWavePlayerCount { get; set; }

	/// <inheritdoc/>
	public uint WaveSpawnCount { get; set; }

	/// <inheritdoc/>
	public Vector3 SpawnOffset { get; set; } = Vector3.zero;

	/// <inheritdoc/>
	public Vector3 SpawnPosition { get; set; } = Vector3.zero;

	/// <inheritdoc/>
	public List<RoomName> SpawnRooms { get; set; } = [];

	/// <inheritdoc/>
	public List<FacilityZone> SpawnZones { get; set; } = [];

	/// <inheritdoc/>
	public List<RoomName> SpawnZoneExcludeRooms { get; set; } = [];

	/// <inheritdoc/>
	public bool SpawnRandomLocation { get; set; }

	/// <inheritdoc/>
	public List<CandyKindID> Candies { get; set; } = [];

	/// <inheritdoc/>
	public override void OnSet([NotNull] Player player)
	{
		if (RoleToSpawn == RoleTypeId.Scp0492)
		{
			player.SetRole(RoleToSpawn, RoleChangeReason.None, RoleSpawnFlags.None);
		}
		else if (player.Role != RoleToSpawn)
		{
			player.SetRole(RoleToSpawn, RoleChangeReason.None, RoleSpawnFlags.All);
		}

		if (ForceSetRole)
		{
			player.SetRoleDelay(RoleToSpawn);
		}

		InterfaceExtensions.Apply((IEffectRole)this, player);
	}

	/// <inheritdoc/>
	public override void OnUnset([NotNull] Player player)
	{
		CustomRoleStore roleStore = CustomRoleStore.GetOrAdd<CustomRoleStore>(player);

		// Reset custom info.
		if (string.IsNullOrEmpty(roleStore.OriginalCustomInfo))
		{
			roleStore.OriginalCustomInfo = string.Empty;
		}

		player.CustomInfo = roleStore.OriginalCustomInfo;
		player.InfoArea = roleStore.OriginalInfoAraea;

		ScaleExtension.SetScale(player, Vector3.one);
		player.Position += Vector3.up;
		player.Gravity = FpcGravityController.DefaultGravity;

		if (roleStore.ShouldResetRole)
		{
			player.SetRoleDelay(ReplaceRole, RoleChangeReason.RoundStart);
		}
	}
}
