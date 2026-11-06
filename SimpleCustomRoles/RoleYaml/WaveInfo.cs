using PlayerRoles;

namespace SimpleCustomRoles.RoleYaml;

public class WaveInfo
{
	public Faction Faction { get; set; } = Faction.Unclassified;

	public int MinRequired { get; set; }

	public bool SkipCheck { get; set; }

	public bool RemoveAfterSpawn { get; set; }
}
