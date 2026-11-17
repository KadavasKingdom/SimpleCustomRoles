using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using SimpleCustomRoles.EventHandlers;

namespace SimpleCustomRoles;

/// <summary>
/// The main entry point for SimpleCustomRoles.
/// </summary>
internal sealed class Main : Plugin<Config>
{
	private static Main? instance;
	private static readonly List<CustomEventsHandler> Handlers =
	[
		new CommonHandler(), new Scp330Handler(), new DropHandler(), new UseHandler(),
		new PocketScaleHandler(), new PocketHandler(), new HurtHandler(),
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
		EnumTxtWriter.WriteAll();

		foreach (var item in Handlers)
		{
			CustomHandlersManager.RegisterEventsHandler(item);
		}

		CustomRoles.RegisterCustomRoles();

		SimpleCustomRoles.Roles.SimpleRole simpleRole = new();
		var yaml = CustomRoleYamlParser.Serializer.Serialize(simpleRole);
		var path = Path.Combine(this.GetConfigDirectory().FullName, "Roles", "SimpleRole.yaml");
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		File.WriteAllText(path, yaml);

		// Harmony.DEBUG = Config.Debug;
		harmony = new(Name);
		harmony.PatchAll();
	}

	/// <inheritdoc/>
	public override void Disable()
	{
		foreach (var item in Handlers)
		{
			CustomHandlersManager.UnregisterEventsHandler(item);
		}

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
	}
}
