using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(SCRComandBase))]
public class OptOutCommand : ICommand
{
    public string Command => "optout";

    public string[] Aliases => ["optoutscr", "quit"];

    public string Description => "Opting out from current Custom Role";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is not PlayerCommandSender pcs)
        {
            response = "Must be coming from Player!";
            return false;
        }
        var player = Player.Get(pcs);
        if (player == null || player.IsHost)
        {
            response = "Must be coming from Player!";
            return false;
        }
        CustomRoleHelpers.UnSetCustomInfoToPlayer(player);
        response = "Sucessfully opted out from Custom Roles";
        return true;
    }
}
