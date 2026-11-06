using InventorySystem.Items.Usables.Scp330;
using LabApi.Loader;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles;

/// <summary>
/// Provides help writing enums.
/// </summary>
internal static class EnumTxtWriter
{
	private static readonly Dictionary<string, string> TxtPair = new()
	{
		{ "DamageType", string.Join("\r\n", Enum.GetNames(typeof(DamageType))) },
		{ "DamageSubType", string.Join("\r\n", Enum.GetNames(typeof(DamageSubType))) },
		{ "UniversalDamageSubType", string.Join("\r\n", Enum.GetNames(typeof(DamageUniversalType))) },
		{ "ItemType", string.Join("\r\n", Enum.GetNames(typeof(ItemType))) },
		{ "Scp096DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp096DamageHandler.AttackType))) },
		{ "Scp049DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp049DamageHandler.AttackType))) },
		{ "MicroHidFiringMode", string.Join("\r\n", Enum.GetNames(typeof(InventorySystem.Items.MicroHID.Modules.MicroHidFiringMode))) },
		{ "ExplosionType", string.Join("\r\n", Enum.GetNames(typeof(ExplosionType))) },
		{ "DisruptorFiringState", string.Join("\r\n", Enum.GetNames(typeof(InventorySystem.Items.Firearms.Modules.DisruptorActionModule.FiringState))) },
		{ "Scp939DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp939DamageType))) },
		{ "Scp3114DamageType", string.Join("\r\n", Enum.GetNames(typeof(Scp3114DamageHandler.HandlerType))) },
		{ "MathOption", string.Join("\r\n", Enum.GetNames(typeof(MathOption))) },
		{ "CustomRoleType", string.Join("\r\n", Enum.GetNames(typeof(CustomRoleType))) },
		{ "RoleTypeId", string.Join("\r\n", Enum.GetNames(typeof(RoleTypeId))) },
		{ "Team", string.Join("\r\n", Enum.GetNames(typeof(Team))) },
		{ "Faction", string.Join("\r\n", Enum.GetNames(typeof(Faction))) },
		{ "LocationSpawnPriority", string.Join("\r\n", Enum.GetNames(typeof(LocationSpawnPriority))) },
		{ "FacilityZone", string.Join("\r\n", Enum.GetNames(typeof(FacilityZone))) },
		{ "RoomName", string.Join("\r\n", Enum.GetNames(typeof(RoomName))) },
		{ "EscapeScenarioType", string.Join("\r\n", Enum.GetNames(typeof(Escape.EscapeScenarioType))) },
		{ "CandyKindID", string.Join("\r\n", Enum.GetNames(typeof(CandyKindID))) },
	};

	private static string Dir => Path.Combine(Path.Combine(Main.Instance?.GetConfigDirectory().FullName, "Roles"), "HelpTXT");

	/// <summary>
	/// Writes all enums into the txts.
	/// </summary>
	public static void WriteAll()
	{
		if (!Directory.Exists(Dir))
		{
			Directory.CreateDirectory(Dir);
		}

		foreach (var item in TxtPair)
		{
			string txt = Path.Combine(Dir, item.Key);
			txt += ".txt";
			if (!File.Exists(txt))
			{
				File.WriteAllText(txt, item.Value);
			}
		}
	}
}
