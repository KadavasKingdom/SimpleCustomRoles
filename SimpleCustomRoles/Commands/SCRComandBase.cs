using CommandSystem;
using LabApi.Features.Permissions;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
public sealed class SCRComandBase : ParentCommand
{
    /// <inheritdoc/>
    public override string Command => "scr";

    /// <inheritdoc/>
    public override string[] Aliases => ["simplecustomrole"];

    /// <inheritdoc/>
    public override string Description => "Interacting with Simple Custom Role";

    /// <inheritdoc/>
    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new ListCommand());
        RegisterCommand(new OptOutCommand());
        RegisterCommand(new PauseCommand());
        RegisterCommand(new ReloadCommand());
        RegisterCommand(new SetCommand());
        RegisterCommand(new ShowCommand());
    }

    /// <inheritdoc/>
    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Please enter a valid subcommand!\n - optout\n";

        if (sender.HasPermissions("scr.list"))
            response += "- list (list all loaded custom roles)\n";

        if (sender.HasPermissions("scr.pause"))
            response += "- pause (pause spawning of custom roles)\n";

        if (sender.HasPermissions("scr.reload"))
            response += "- reload (reloading all custom roles)\n";

        if (sender.HasPermissions("scr.set"))
            response += "- set (set id(s) to custom role)\n";

        if (sender.HasPermissions("scr.show"))
            response += "- show (show all current role)\n";

        return false;
    }

}
