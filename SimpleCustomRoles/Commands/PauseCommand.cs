using CommandSystem;
using LabApi.Features.Permissions;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
public class PauseCommand : ICommand
{
    public string Command => "pause";
    public string[] Aliases => [];
    public string Description => "Pause or resume custom roles";
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions("scr.pause"))
        {
            response = "You do not have a permission for this";
            return false;
        }

        if (arguments.Count == 1)
        {
            var arg0 = arguments.At(0);

            if (arg0 == "off" || arg0 == "false" || arg0 == "0")
            {
                Main.Instance.Config.IsPaused = true;
                response = "Custom Roles spawn are now paused";
                return true;
            }
            if (arg0 == "on" || arg0 == "true" || arg0 == "1")
            {
                Main.Instance.Config.IsPaused = false;
                response = "Custom Roles spawn are now resumed";
                return true;
            }
        }
        else
        {
            Main.Instance.Config.IsPaused = !Main.Instance.Config.IsPaused;
            response = "Custom Roles spawn are now " + (Main.Instance.Config.IsPaused ? "paused" : "resumed");
            return true;
        }
        response = "Something off";
        return false;
    }
}
