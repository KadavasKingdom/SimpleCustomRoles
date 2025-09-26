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

    private readonly ServerHandler serverHandler = new();
    private readonly PocketHandler pocketHandler = new();
    private readonly PlayerHandler playerHandler = new();
    private readonly Scp0492Handler scp0492Handler = new();
    private readonly Scp049Handler scp049Handler = new();
    private readonly Scp096Handler scp096Handler = new();
    private readonly Scp173Handler scp173Handler = new();
    private readonly Scp330Handler scp330Handler = new();

    public List<RoleBaseGroup> RoleGroups = [];

    private Harmony Harmony;

    public override void Enable()
    {
        Instance = this;
        CustomDataStoreManagerExtended.EnsureExists<CustomRoleInfoStorage>();
        HelperTxts.WriteAll();
        Logic.Init();

        CustomHandlersManager.RegisterEventsHandler(serverHandler);
        CustomHandlersManager.RegisterEventsHandler(playerHandler);
        CustomHandlersManager.RegisterEventsHandler(pocketHandler);
        CustomHandlersManager.RegisterEventsHandler(scp049Handler);
        CustomHandlersManager.RegisterEventsHandler(scp0492Handler);
        CustomHandlersManager.RegisterEventsHandler(scp096Handler);
        CustomHandlersManager.RegisterEventsHandler(scp173Handler);
        CustomHandlersManager.RegisterEventsHandler(scp330Handler);

        StatusEffectBase.OnEnabled += SubHandle.StatusEffectBase_OnEnabled;
        Harmony.DEBUG = Config.Debug;
        Harmony = new("SimpleCustomRole");
        Harmony.PatchAll();
    }


    public override void Disable()
    {
        StatusEffectBase.OnEnabled -= SubHandle.StatusEffectBase_OnEnabled;

        CustomHandlersManager.RegisterEventsHandler(serverHandler);
        CustomHandlersManager.RegisterEventsHandler(playerHandler);
        CustomHandlersManager.RegisterEventsHandler(pocketHandler);
        CustomHandlersManager.RegisterEventsHandler(scp049Handler);
        CustomHandlersManager.RegisterEventsHandler(scp0492Handler);
        CustomHandlersManager.RegisterEventsHandler(scp096Handler);
        CustomHandlersManager.RegisterEventsHandler(scp173Handler);
        CustomHandlersManager.RegisterEventsHandler(scp330Handler);

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
