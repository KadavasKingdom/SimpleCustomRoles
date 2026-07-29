using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using LiteNetLib4Mirror.Open.Nat;
using MapGeneration;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using Utils;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
public class RigCommand : ICommand, IUsageProvider
{
    public string Command => "rig";
    public string[] Aliases => [];
    public string Description => "Forces a certain role to spawn this round (only usable during waiting for players)";
    public string[] Usage => ["RoleName"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        // inherit permission from set, does basically the same thing anyways
        if (!sender.HasPermissions("scr.set"))
        {
            response = "You dont have permission!";
            return false;
        }
        
        if (RoundSummary.RoundInProgress())
        {
            response = "Round is already in progress!";
            return false;
        }

        if (arguments.Count != 1)
        {
            response = "Please supply a role.\nUsage: " + arguments.Array[0] + " " + this.DisplayCommandUsage();
            return false;
        }
        var roleName = arguments.At(0);
        var role = RolesLoader.RoleInfos.FirstOrDefault(x => x.Rolename == roleName);
        if (role == default)
        {
            response = $"No role exists named {roleName}!";
            return false;
        }

        Main.Instance.RiggedRoles.Add(role);
        response = "Done!";
        return true;
    }
}
