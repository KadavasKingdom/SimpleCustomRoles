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

/// <summary>
/// The main entry point for SimpleCustomRoles.
/// </summary>
internal sealed class Main : Plugin<Config>
{
	private static Main? instance;
	private static readonly List<CustomEventsHandler> Handlers =
	[
		new ServerHandler(), new PocketHandler(), new PlayerHandler(), new EscapeHandler(),
		new Scp0492Handler(), new Scp049Handler(), new Scp079Handler(), new Scp096Handler(),
		new Scp173Handler(), new Scp330Handler(), new Scp939Handler(), new ObjectHandler(),
	];

	/// <summary>
	/// Gets the instance of SimpleCustomRoles.
	/// </summary>
	public static Main Instance
	{
		get
		{
			if (instance == null)
			{
				throw new ArgumentNullException(nameof(Instance));
			}

			return instance;
		}
	}

	/// <summary>
	/// Gets the in wave custom roles.
	/// </summary>
	public static List<CustomRoleBaseInfo> InWaveRoles { get; } = [];

	/// <summary>
	/// Gets the scp specific custom roles.
	/// </summary>
	public static List<CustomRoleBaseInfo> ScpSpecificRoles { get; } = [];

	/// <summary>
	/// Gets the escape custom roles.
	/// </summary>
	public static List<CustomRoleBaseInfo> EscapeRoles { get; } = [];

	/// <summary>
	/// Gets the rigged custom roles.
	/// </summary>
	public static List<CustomRoleBaseInfo> RiggedRoles { get; } = [];

	/// <summary>
	/// Gets the role groups.
	/// </summary>
	public static List<RoleBaseGroup> RoleGroups { get; } = [];

	private Harmony? harmony;

	/// <inheritdoc/>
	public override string Author => "SlejmUr";

	/// <inheritdoc/>
	public override string Name => "SimpleCustomRoles";

	/// <inheritdoc/>
	public override Version Version => Versioning.Version;

	/// <inheritdoc/>
	public override string Description => "Add simple YAML Support for creating custom roles.";

	/// <inheritdoc/>
	public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

	/// <inheritdoc/>
	public override void Enable()
	{
		instance = this;
		CustomDataStoreManagerExtended.EnsureExists<CustomRoleInfoStorage>();
		EnumTxtWriter.WriteAll();
		Logic.Init();

		foreach (var item in Handlers)
		{
			CustomHandlersManager.RegisterEventsHandler(item);
		}

		StatusEffectBase.OnEnabled += SubHandle.StatusEffectBase_OnEnabled;

		// Harmony.DEBUG = Config.Debug;
		harmony = new(Name);
		harmony.PatchAll();
	}

	/// <inheritdoc/>
	public override void Disable()
	{
		StatusEffectBase.OnEnabled -= SubHandle.StatusEffectBase_OnEnabled;

		foreach (var item in Handlers)
		{
			CustomHandlersManager.UnregisterEventsHandler(item);
		}

		Logic.UnInit();
		RolesLoader.Clear();
		harmony?.UnpatchAll(Name);
		harmony = null;
		instance = null;
	}

	/// <inheritdoc/>
	public override void LoadConfigs()
	{
		if (!this.TryLoadConfig(ConfigFileName, out Config? config, true))
		{
			CL.Warn("Failed to load the configuration file, using default values.");
			config = new();
		}

		Config = config;
		var list = this.LoadConfig<List<RoleBaseGroup>>("RoleGroups.yml", config.UseGlobalDir);
		RoleGroups.Clear();

		if (list != null)
		{
			RoleGroups.AddRange(list);
		}
	}
}
