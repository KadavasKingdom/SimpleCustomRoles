using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;

namespace SimpleCustomRoles.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class TempShowEffect : ICommand
{
	/// <inheritdoc/>
	public string Command => "showeffects";

	/// <inheritdoc/>
	public string[] Aliases => ["showeffects", "geteffects", "scr_ef"];

	/// <inheritdoc/>
	public string Description => "Show your current Effects";

	/// <inheritdoc/>
	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (sender is not PlayerCommandSender pcs)
		{
			response = "Must be coming from Player!";
			return false;
		}

		var player = Player.Get(pcs);
		if (player == null)
		{
			response = "Must be coming from Player!";
			return false;
		}

		response = "Your effects: \n";
		foreach (var effect in player.ActiveEffects)
		{
			string effectName = effect.ToString();
			response += effectName + $" (d: {effect.Duration} i:{effect.Intensity})" + "\n";
		}

		return true;
	}
}
