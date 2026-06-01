using PlayerRoles;
using SimpleCustomRoles.RoleYaml;
using System.ComponentModel;

namespace SimpleCustomRoles;

public class Config
{
    public bool Debug { get; set; }
    public bool DebugEscape { get; set; } = true;
    public bool UseGlobalDir { get; set; }
    public bool IsPaused { get; set; }
    public ushort SpectatorBroadcastTime { get; set; } = 7;
    public bool UsePlayerPercent { get; set; }

    [Description("Biases role generation towards rarer roles. Recommended 1-6.")]
    public float SpawnRateBias { get; set; } = 1f;

    [Description("Multiplies weights of all roles. Doesn't do much outside of reducing the chance of NO custom role spawning.")]
    public float SpawnRateMultiplier { get; set; } = 1f;

    [Description("Multiplies all SpawnAmounts. Rounds down after calculation (if this is 1.75, 1*1.75 = 1)")]
    public float SpawnAmountMultiplier { get; set; } = 1f;

    [Description("Changes roles with SpawnAmount A to instead have SpawnAmount B. SpawnAmountMultiplier only applies to roles with spawn amounts that are not in this list.")]
    public Dictionary<int, int> SpawnAmountDictionary { get; set; } = new Dictionary<int, int>() {
        { 696969, 420 },
    };

    [Description("Weight for each role determining the chance of no custom role spawning at all. Defaults to 2000.")]
    public Dictionary<RoleTypeId, int> NoCustomRoleChance { get; set; } = new()
    {
        { RoleTypeId.ClassD, 2000 },
        { RoleTypeId.Scp0492, 5000 }
    };

    [Description("For CustomItemsAPI use \"/lci give {0} {1}\" for exiled use \"/ci give {0} {1}\"")]
    public string CustomItemCommand { get; set; } = "/lci give {0} {1}";
    public bool CustomItemUseName { get; set; } = true;

    [Description("Showing spawn message in console")]
    public bool ShowSpawnMessage { get; set; }

    public Dictionary<EscapeConfig, RoleTypeId> EscapeConfigs { get; set; } = new()
    {
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosConscript,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosMarauder,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosRepressor,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.ChaosRifleman,
                ShouldBeCuffer = true,
            },
            RoleTypeId.NtfSpecialist
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfCaptain,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosMarauder
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfPrivate,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfSergeant,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
        {
            new()
            {
                EscapeRole = RoleTypeId.NtfSpecialist,
                ShouldBeCuffer = true,
            },
            RoleTypeId.ChaosConscript
        },
    };
}

