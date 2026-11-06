using CommandSystem;
using LabApi.Features.Permissions;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(SCRComandBase))]
internal sealed class RigCommand : ICommand, IUsageProvider
{
	/// <inheritdoc/>
	public string Command => "rig";

	/// <inheritdoc/>
	public string[] Aliases => [];

	/// <inheritdoc/>
	public string Description => "Forces a certain role to spawn this round (only usable during waiting for players)";

	/// <inheritdoc/>
	public string[] Usage => ["RoleName"];

	/// <inheritdoc/>
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

		Main.RiggedRoles.Add(role);
		response = "Done!";
		return true;
	}
}
