using CommandSystem;
using LabApi.Features.Wrappers;
using SimpleCustomRoles.Helpers;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
public class OptOutCommand : ICommand
{
    public string Command => "optout";

    public string[] Aliases => ["optoutscr"];

    public string Description => "Opt out from your current Custom Role";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
        if (player == null || player.IsHost)
        {
            response = "Must be coming from a player!";
            return false;
        }

        if(player.ReferenceHub.roleManager.CurrentRole.ActiveTime > Main.Instance.Config.OptOutValidTime)
        {
            response = $"You cannot opt out of a Custom Role longer than {Main.Instance.Config.OptOutValidTime}s after spawning!";
            return false;
        }

        bool result = CustomRoleHelpers.UnSetCustomInfoToPlayer(player, out response, fromOptOut: true);

        CL.Info($"Opting out player: {player.PlayerId} Result: {result}");

        if (!result)
            return false;

        response = "Sucessfully opted out from your custom role!";
        return true;
    }
}
