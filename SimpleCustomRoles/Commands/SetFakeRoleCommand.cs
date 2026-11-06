using CommandSystem;
using PlayerRoles;
using Utils;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class SetFakeRoleCommand : ICommand, IUsageProvider
{
	/// <inheritdoc/>
	public string Command => "setfakerole";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "Faking a role to the players. (None will remove it)";

	/// <inheritdoc/>
	public string[] Usage => ["%role%", "%player%"];

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (arguments.Count != 2)
		{
			response = "This command equires 2 arguments! " + HelpProviderExtensions.DisplayCommandUsage(this);
			return false;
		}

		string role = arguments.At(0);
		if (!Enum.TryParse(role, true, out RoleTypeId roleType))
		{
			response = "Cannot parse the RoleTypeId!";
			return false;
		}

		List<Player> players = [.. RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _).Select(Player.Get).Where(x => x is not null).Select(x => x!)];
		if (players.Count == 0)
		{
			response = "No players found!";
			return false;
		}

		foreach (var item in players)
		{
			if (roleType == RoleTypeId.None)
			{
				item.RemoveFakeRole();
			}
			else
			{
				item.AddFakeRole(roleType);
			}
		}

		response = "Done";
		return true;
	}
}
