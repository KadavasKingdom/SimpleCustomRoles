using CommandSystem;
using LabApi.Features.Permissions;
using Utils;

namespace SimpleCustomRoles.Commands;

/// <summary>
/// Represent a command for setting custom roles to senderPlayer.
/// </summary>
[CommandHandler(typeof(SCRParentCommand))]
internal sealed class SetCommand : ICommand, IUsageProvider
{
	/// <inheritdoc/>
	public string Command => "set";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "Set the custom role with given input";

	/// <inheritdoc/>
	public string[] Usage => ["RoleName", "%player% (optional)"];

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (!sender.HasPermissions("scr.set"))
		{
			response = "You dont have permission!";
			return false;
		}

		if (arguments.Count < 1)
		{
			response = "To execute this command provide at least 1 arguments!\nUsage: " + this.DisplayCommandUsage();
			return false;
		}

		// Checking and getting all players to set.
		List<Player> players = [];
		Player? senderPlayer = Player.Get(sender);

		if (senderPlayer != null)
		{
			players.Add(senderPlayer);
		}

		if (arguments.Count == 2)
		{
			players = [.. RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _).Select(Player.Get).Where(x => x is not null).Select(x => x!)];
		}

		if (players.Count == 0)
		{
			response = "No valid players found to be able to set their roles!";
			return false;
		}

		string roleName = arguments.At(0);

		bool reset = roleName == ".";
		if (!reset && !CustomRoles.HasRoleName(roleName))
		{
			response = $"Role with name {roleName} not exist!";
			return false;
		}

		foreach (var item in players)
		{
			if (reset)
			{
				CustomRoles.UnsetCustomRole(item);
			}
			else
			{
				CustomRoles.SetCustomRole(item, roleName);
			}
		}

		response = "Done!";
		return true;
	}
}
