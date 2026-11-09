using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.Commands;

/// <summary>
/// Represent a command listing all loaded custom roles.
/// </summary>
[CommandHandler(typeof(SCRParentCommand))]
internal sealed class ListCommand : ICommand
{
	/// <inheritdoc/>
	public string Command => "list";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "List the Custom Role Names";

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.HasPermissions("scr.list"))
		{
			response = "You do not have a permission for this";
			return false;
		}

		response = $"Loaded from YAML: {CustomRoles.YamlStore.Count}";
		foreach (var item in CustomRoles.YamlStore)
		{
			string nameOnRA = item is IDisplayRole displayRole ? displayRole.NameRA : string.Empty;

			response += $"\n{item.RoleName} [{nameOnRA}]";
		}

		response += $"\nLoaded from Assemblies: {CustomRoles.RolesStore.Count}";
		foreach (var item in CustomRoles.RolesStore)
		{
			string nameOnRA = item is IDisplayRole displayRole ? displayRole.NameRA : string.Empty;

			response += $"\n{item.RoleName} [{nameOnRA}]";
		}

		return true;
	}
}
