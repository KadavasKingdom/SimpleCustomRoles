using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.Interfaces;

namespace SimpleCustomRoles.Commands;

/// <summary>
/// Represent a command to show current alive custom roles.
/// </summary>
[CommandHandler(typeof(SCRParentCommand))]
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

		List<Player> players = CustomRoles.GetPlayers();
		if (players.Count == 0)
		{
			response = $"There is no Custom Roles.";
			return true;
		}

		response = "Currently Playing roles:";
		foreach (var player in players)
		{
			if (CustomRoles.TryGetCustomRole(player, out var role) && role != null)
			{
				string nameOnRA = role is IDisplayRole displayRole ? displayRole.NameRA : string.Empty;

				response += $"\n{role.RoleName} [{nameOnRA}]: [Id]{player.PlayerId} [Name]{player.Nickname}";
			}
		}

		return true;
	}
}
