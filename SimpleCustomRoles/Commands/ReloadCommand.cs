using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.Handler;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
internal sealed class ReloadCommand : ICommand
{
	/// <inheritdoc/>
	public string Command => "reload";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "Reload the Custom Role Names";

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.HasPermissions("scr.reload"))
		{
			response = "You dont have permission!";
			return false;
		}

		ServerHandler.ReloadRoles();
		response = "Roles Reloaded!";
		return true;
	}
}
