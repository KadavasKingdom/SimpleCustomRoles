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

    public string Description => "set role to fakerole";

    public string[] Usage => ["%role%", "%player%"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2)
        {
            response = "req 2 arg!";
            return false;
        }
        string role = arguments.At(0);
        if (!Enum.TryParse(role, true, out RoleTypeId roleType))
        {
            response = "cannot parse roletypeid!";
            return false;
        }

        List<Player> players = [.. RAUtils.ProcessPlayerIdOrNamesList(arguments, 1, out _).Select(Player.Get)];
        if (players.Count == 0)
        {
            response = "No players!";
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
