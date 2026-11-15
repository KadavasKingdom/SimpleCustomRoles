using LabApi.Events;
using SimpleCustomRoles.Arguments;
using SimpleCustomRoles.Roles;

namespace SimpleCustomRoles;

#pragma warning disable CA1003 // Use generic event handler instances

/// <summary>
/// Handles all events related to custom roles.
/// </summary>
public static class CustomRolesEvents
{
	/// <summary>
	/// Gets called when a custom role is registered.
	/// </summary>
	public static event LabEventHandler<CustomRoleEventArgs>? Registered;

	/// <summary>
	/// Gets called when a custom role is unregistered.
	/// </summary>
	public static event LabEventHandler<CustomRoleEventArgs>? Unregistered;

	/// <summary>
	/// Gets called when a custom role is added to yaml store.
	/// </summary>
	public static event LabEventHandler<CustomRoleEventArgs>? YamlAdded;

	/// <summary>
	/// Gets called when a custom role is set to player.
	/// </summary>
	public static event LabEventHandler<CustomRolePlayerEventArgs>? SetRole;

	/// <summary>
	/// Gets called when a custom role is unregistered.
	/// </summary>
	public static event LabEventHandler<CustomRolePlayerEventArgs>? UnSetRole;

	/// <inheritdoc cref="Registered"/>
	internal static void OnRegistered(CustomRoleBase customRole)
		=> Registered.InvokeEvent(new(customRole));

	/// <inheritdoc cref="Unregistered"/>
	internal static void OnUnregistered(CustomRoleBase customRole)
		=> Unregistered.InvokeEvent(new(customRole));

	/// <inheritdoc cref="YamlAdded"/>
	internal static void OnYamlAdded(CustomRoleBase customRole)
		=> YamlAdded.InvokeEvent(new(customRole));

	/// <inheritdoc cref="SetRole"/>
	internal static void OnSetRole(CustomRoleBase customRole, Player player)
		=> SetRole.InvokeEvent(new(player, customRole));

	/// <inheritdoc cref="UnSetRole"/>
	internal static void OnUnSetRole(CustomRoleBase customRole, Player player)
		=> UnSetRole.InvokeEvent(new(player, customRole));
}
