using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
internal sealed class ShowCommand : ICommand
{
	/// <inheritdoc/>
	public string Command => "show";

	/// <inheritdoc/>
	public string[] Aliases => ["current", "cur"];

	/// <inheritdoc/>
	public string Description => "List currently what player is which role.";

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.HasPermissions("scr.show"))
		{
			response = "You dont have permission!";
			return false;
		}

		var players = CustomRoleHelpers.GetPlayers();
		if (players.Count == 0)
		{
			response = $"There is no Custom Roles.";
			return true;
		}

		response = "Currently Playing roles:";
		foreach (var player in players)
		{
			if (CustomRoleHelpers.TryGetCustomRole(player, out var role) && role != null)
			{
				response += $"\n{role.Rolename} [{role.Display.RARoleName}]: [Id]{player.PlayerId} [Name]{player.Nickname}";
			}
		}

		return true;
	}
}
