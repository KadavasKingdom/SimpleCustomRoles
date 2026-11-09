using CommandSystem;
using LabApi.Features.Permissions;

namespace SimpleCustomRoles.Commands;

/// <summary>
/// Represent a command to reload yaml roles.
/// </summary>
[CommandHandler(typeof(SCRParentCommand))]
internal sealed class ReloadCommand : ICommand
{
	/// <inheritdoc/>
	public string Command => "reload";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "Reload the Custom Roles from stored YAML files.";

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.HasPermissions("scr.reload"))
		{
			response = "You dont have permission!";
			return false;
		}

		CustomRoles.LoadRoles();
		response = "Roles Reloaded!";
		return true;
	}
}
