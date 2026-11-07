using LabApi.Loader;
using SimpleCustomRoles.Roles;
using SimpleCustomRoles.Store;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SimpleCustomRoles;

/// <summary>
/// Provides a management for custom roles.
/// </summary>
public static class CustomRoles
{
	/// <summary>
	/// The type mapping.
	/// </summary>
	public static readonly Dictionary<string, Type> TypeMapping = new()
	{
		[string.Empty] = typeof(SimpleRole),
	};

	/// <summary>
	/// Gets the custom roles.
	/// </summary>
	public static List<CustomRoleBase> RolesStore { get; } = [];

	/// <summary>
	/// Gets the player custom role store.
	/// </summary>
	public static List<CustomRoleStore> PlayerRolesStore { get; } = [];

	/// <summary>
	/// Tries to get the custom role store.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="store">The store.</param>
	/// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
	public static bool TryGetCustomStore(Player? player, [NotNullWhen(true)] out CustomRoleStore? store)
	{
		if (player == null)
		{
			store = null;
			return false;
		}

		store = PlayerRolesStore.FirstOrDefault(x => x.Owner == player);
		return store != null;
	}

	/// <summary>
	/// Tries to get the custom role.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="customRole">The custom role.</param>
	/// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
	public static bool TryGetCustomRole(Player? player, [NotNullWhen(true)] out CustomRoleBase? customRole)
	{
		customRole = null;
		if (player == null)
		{
			return false;
		}

		if (!TryGetCustomStore(player, out CustomRoleStore? store))
		{
			return false;
		}

		return store.TryGetCustomRole(out customRole);
	}

	/// <summary>
	/// Register custom roles with the Calling Assembly.
	/// </summary>
	public static void RegisterCustomRoles()
	{
		Assembly assembly = Assembly.GetCallingAssembly();
#if !DEBUG
		if (assembly == typeof(CustomRoleBase).Assembly)
		{
			return;
		}
#endif
		List<Type> types = [.. assembly.GetTypes().
			Where(item =>
				!item.IsAbstract &&
				typeof(CustomRoleBase).IsAssignableFrom(item))];

		RegisterCustomRoles([..types]);
	}

	/// <summary>
	/// Register only <paramref name="types"/> as custom roles.
	/// </summary>
	/// <param name="types">Parameter array of the <see cref="Type"/> of a <see cref="CustomRoleBase"/>.</param>
	public static void RegisterCustomRoles(params Type[] types)
	{
		if (types == null)
		{
			return;
		}

		foreach (Type type in types)
		{
			RegisterCustomRole(type);
		}
	}

	/// <summary>
	/// Register individual <paramref name="type"/> as a custom role.
	/// </summary>
	/// <param name="type">The <see cref="Type"/> of a <see cref="CustomRoleBase"/>.</param>
	public static void RegisterCustomRole(Type type)
	{
		if (type == null)
		{
			return;
		}

		if (type.Equals(null))
		{
			return;
		}

		CL.Debug(type, Main.Instance.Config.Debug);
		CustomRoleBase customRole = (CustomRoleBase)Activator.CreateInstance(type);
		RegisterCustomRole(customRole);
	}

	/// <summary>
	/// Register <paramref name="customRoleBase"/> as a custom role.
	/// </summary>
	/// <param name="customRoleBase">The new <see cref="CustomRoleBase"/>.</param>
	public static void RegisterCustomRole(this CustomRoleBase? customRoleBase)
	{
		if (customRoleBase == null)
		{
			return;
		}

		RolesStore.Add(customRoleBase);
		TypeMapping.Add(customRoleBase.ParserClassName, customRoleBase.GetType());
		customRoleBase.OnRegistered();
		/*
		CustomRolesEvents.OnRegistered(customRoleBase);
		*/
	}

	public static void Serialize<T>(T role)
		where T : CustomRoleBase
	{
		string name = typeof(T).Name;

		string yaml = CustomRoleYamlParser.Serializer.Serialize(role);
		string path = Path.Combine(ConfigurationLoader.GetConfigDirectory(Main.Instance, true).FullName, $"{name}.yaml");
		File.WriteAllText(path, yaml);
	}

	public static T Read<T>()
		where T : CustomRoleBase
	{
		string name = typeof(T).Name;
		string path = Path.Combine(ConfigurationLoader.GetConfigDirectory(Main.Instance, true).FullName, $"{name}.yaml");
		string data = File.ReadAllText(path);
		return CustomRoleYamlParser.Deserializer.Deserialize<T>(data);
	}

	public static CustomRoleBase? ReadYamlAsBase<T>()
		where T : CustomRoleBase
	{
		string name = typeof(T).Name;
		string path = Path.Combine(ConfigurationLoader.GetConfigDirectory(Main.Instance, true).FullName, $"{name}.yaml");
		string data = File.ReadAllText(path);
		return CustomRoleYamlParser.Deserializer.Deserialize<CustomRoleBase>(data);
	}
}
