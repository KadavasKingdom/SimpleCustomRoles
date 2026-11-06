using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
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

		response = "Roles:";
		foreach (var item in RolesLoader.RoleInfos)
		{
			response += $"\n{item.Rolename} [{item.Display.RARoleName}]";
		}

		response += $"\nLoaded: {RolesLoader.RoleInfos.Count}";
		return true;
	}
}
