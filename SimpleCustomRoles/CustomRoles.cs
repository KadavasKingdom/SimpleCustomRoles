using LabApi.Loader;
using SimpleCustomRoles.Roles;
using SimpleCustomRoles.Store;
using System.Diagnostics.CodeAnalysis;

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

	public static void SerializeYaml<T>(T role)
		where T : CustomRoleBase
	{
		string name = typeof(T).Name;

		string yaml = CustomRoleYamlParser.Serializer.Serialize(role);
		string path = Path.Combine(ConfigurationLoader.GetConfigDirectory(Main.Instance, true).FullName, $"{name}.yaml");
		File.WriteAllText(path, yaml);
	}

	public static T ReadYaml<T>()
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
