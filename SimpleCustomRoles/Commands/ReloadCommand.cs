using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.Handler;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
public class ReloadCommand : ICommand
{
    public string Command => "reload";

    public string[] Aliases => [];

    public string Description => "Reload the Custom Role Names";

    public bool SanitizeResponse => true;

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions("scr.reload"))
        {
            response = "You dont have permission!";
            return false;
        }
        ServerHandler.ReloadRoles();
        response = "Roles Reloaded!";
        return true;
    }
}
