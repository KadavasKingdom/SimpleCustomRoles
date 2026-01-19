using CustomPlayerEffects;
using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleGroup;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.SSS;

namespace SimpleCustomRoles;

internal class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "SimpleCustomRoles";
    public override Version Version => new(0, 5, 9);
    public override string Description => "Add simple YAML Support for creating custom roles.";
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
    #endregion

    public List<CustomRoleBaseInfo> InWaveRoles = [];
    public List<CustomRoleBaseInfo> ScpSpecificRoles = [];
    public List<CustomRoleBaseInfo> EscapeRoles = [];

    private readonly List<CustomEventsHandler> handlers = 
    [
        new ServerHandler(), new PocketHandler(), new PlayerHandler(), new EscapeHandler(),
        new Scp0492Handler(), new Scp049Handler(), new Scp079Handler(), new Scp096Handler(),
        new Scp173Handler(), new Scp330Handler(),
    ];

    public List<RoleBaseGroup> RoleGroups = [];

    private Harmony Harmony;

    public override void Enable()
    {
        Instance = this;
        CustomDataStoreManagerExtended.EnsureExists<CustomRoleInfoStorage>();
        HelperTxts.WriteAll();
        Logic.Init();

        foreach (var item in handlers)
        {
            CustomHandlersManager.RegisterEventsHandler(item);
        }

        StatusEffectBase.OnEnabled += SubHandle.StatusEffectBase_OnEnabled;
        //Harmony.DEBUG = Config.Debug;
        Harmony = new("SimpleCustomRole");
        Harmony.PatchAll();
    }


    public override void Disable()
    {
        StatusEffectBase.OnEnabled -= SubHandle.StatusEffectBase_OnEnabled;

        foreach (var item in handlers)
        {
            CustomHandlersManager.UnregisterEventsHandler(item);
        }

        Logic.UnInit();
        RolesLoader.Clear();
        Harmony.UnpatchAll("SimpleCustomRole");
        Instance = null;
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        var list = this.LoadConfig<List<RoleBaseGroup>>("RoleGroups.yml");
        if (list != null)
            RoleGroups = list;
        else
            RoleGroups = [];
    }
}
