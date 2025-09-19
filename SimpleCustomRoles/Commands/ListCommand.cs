using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
public class ListCommand : ICommand
{
    public string Command => "list";
    public string[] Aliases => [];
    public string Description => "List the Custom Role Names";
    public bool Execute(ArraySegment<string> _, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions("scr.list"))
        {
            response = "You do not have a permission for this";
            return false;
        }

        response = "Roles:";
        foreach (var item in RolesLoader.RoleInfos)
        {
            response += $"\n{item.Rolename} [{item.Display.RARoleName}]";
        }
        return true;
    }
}
