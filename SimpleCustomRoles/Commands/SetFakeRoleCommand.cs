using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Utils;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class SetFakeRoleCommand : ICommand, IUsageProvider
{
    public string Command => "setfakerole";

    public string[] Aliases => [];

    public string Description => "Faking a role to the players. (None will remove it)";

    public string[] Usage => ["%role%", "%player%"];

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

        List<Player> players = [.. RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _).Select(Player.Get)];
        if (players.Count == 0)
        {
            response = "No players found!";
            return false;
        }

        foreach (var item in players)
        {
            if (roleType == RoleTypeId.None)
                item.RemoveFakeRole();
            else
                item.AddFakeRole(roleType);
        }

        response = "Done";
        return true;
    }
}
