using LabApi.Features.Stores;
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
	/// Gets the custom roles from yaml.
	/// </summary>
	public static List<CustomRoleBase> YamlStore { get; } = [];

	/// <summary>
	/// Tries to get the custom role store.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="store">The store.</param>
	/// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
	public static bool TryGetCustomStore(this Player? player, [NotNullWhen(true)] out CustomRoleStore? store)
	{
		if (player == null)
		{
			store = null;
			return false;
		}

		store = CustomDataStore.GetOrAdd<CustomRoleStore>(player);
		return store != null;
	}

	/// <summary>
	/// Tries to get the custom role.
	/// </summary>
	/// <param name="player">The player.</param>
	/// <param name="customRole">The custom role.</param>
	/// <returns><see langword="true"/> if success; otherwise <see langword="false"/>.</returns>
	public static bool TryGetCustomRole(this Player? player, [NotNullWhen(true)] out CustomRoleBase? customRole)
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

	/// <summary>
	/// Loads all role from config storage.
	/// </summary>
	public static void LoadRoles()
	{
		string rolesPath = Path.Combine(Main.Instance.GetConfigDirectory(Main.Instance.Config.UseGlobal).FullName, "Roles");
		if (!Directory.Exists(rolesPath))
		{
			Directory.CreateDirectory(rolesPath);
			return;
		}

		foreach (string file in Directory.GetFiles(rolesPath, "*.yaml", SearchOption.AllDirectories))
		{
			if (file.Contains(".disable") || file.Contains(".d"))
			{
				continue;
			}

			if (!file.Contains(".yaml"))
			{
				continue;
			}

			try
			{
				CustomRoleBase info = CustomRoleYamlParser.Deserializer.Deserialize<CustomRoleBase>(File.ReadAllText(file));
				YamlStore.Add(info);
			}
			catch (Exception ex)
			{
				CL.Error($"Loading role errored on file {file}, error: {ex}");
			}
		}
	}

	/// <summary>
	/// Set custom role for <paramref name="player"/>.
	/// </summary>
	/// <param name="player">The player to set to.</param>
	/// <param name="roleName">The name of the custom role.</param>
	public static void SetCustomRole([NotNull] this Player player, string roleName)
	{
		if (string.IsNullOrEmpty(roleName))
		{
			return;
		}

		CustomRoleBase? role = YamlStore.FirstOrDefault(x => x.RoleName == roleName);

		// If role not found in yaml store, check added assembly.
		role ??= RolesStore.FirstOrDefault(x => x.RoleName == roleName);

		if (role == null)
		{
			return;
		}

		SetCustomRole(player, role);
	}

	/// <summary>
	/// Set custom role for <paramref name="player"/>.
	/// </summary>
	/// <param name="player">The player to set to.</param>
	/// <param name="customRoleBase">The custom role.</param>
	public static void SetCustomRole([NotNull] this Player player, [NotNull] CustomRoleBase customRoleBase)
	{
		CustomRoleStore customRoleStore = CustomDataStore.GetOrAdd<CustomRoleStore>(player);
		customRoleStore.SetCustomRole(customRoleBase);
		customRoleBase.OnSet(player);
	}

	/// <summary>
	/// Unset custom role for <paramref name="player"/>.
	/// </summary>
	/// <param name="player">The player to set to.</param>
	public static void UnsetCustomRole([NotNull] this Player player)
	{
		CustomRoleStore customRoleStore = CustomDataStore.GetOrAdd<CustomRoleStore>(player);
		customRoleStore.TryGetCustomRole(out var role);
		customRoleStore.SetCustomRole(null);
		role?.OnUnset(player);
	}
}
