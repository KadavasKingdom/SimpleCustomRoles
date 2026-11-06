using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using Utils;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
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

		List<Player> players = [];
		var player = Player.Get(sender);
		if (arguments.Count == 1)
		{
			response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + this.DisplayCommandUsage();
			return false;
		}

		if (player == null)
		{
			response = "Sender is not a player!";
			return false;
		}

		players.Add(player);
		string roleName = arguments.At(0);
		if (arguments.Count == 2)
		{
			players = [.. RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _).Select(Player.Get).Where(x => x is not null).Select(x => x!)];
		}

		if (players.Count == 0)
		{
			response = "No players!";
			return false;
		}

		bool reset = roleName == ".";
		var role = RolesLoader.RoleInfos.FirstOrDefault(x => x.Rolename == roleName);
		if (!reset && role == default)
		{
			response = $"Role with name {roleName} not exist!";
			return false;
		}

		foreach (var item in players)
		{
			if (reset)
			{
				CustomRoleHelpers.UnSetCustomInfoToPlayer(item);
			}
			else
			{
				CustomRoleHelpers.SetFromCMD(item, role);
			}
		}

		response = "Done!";
		return true;
	}
}
