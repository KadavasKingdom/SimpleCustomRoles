using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class SpawnInfo
{
    [Description("Role spawning weight. 0 = will never spawn.")]
    public int SpawnChance { get; set; } = 0;

    [Description("Role spawn ammount")]
    public int SpawnAmount { get; set; } = 0;

    [Description("Minimum player count this role should spawn (-1 means no minimum!)")]
    public int MinimumPlayers { get; set; } = -1;

    [Description("Maximum player count this role should spawn (-1 means no maximum!)")]
    public int MaximumPlayers { get; set; } = -1;

    [Description("Denying editing the Spawn Chance by any means.")]
    public bool DenyChance { get; set; } = false;
}
