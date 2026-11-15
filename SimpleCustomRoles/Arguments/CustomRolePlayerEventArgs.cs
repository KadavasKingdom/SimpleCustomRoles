using LabApi.Events.Arguments.Interfaces;
using SimpleCustomRoles.Roles;

namespace SimpleCustomRoles.Arguments;

/// <summary>
/// Represents the arguments for the common custom role events.
/// </summary>
/// <param name="player">The player calling the event.</param>
/// <param name="customRole">The custom role.</param>
public class CustomRolePlayerEventArgs(Player player, CustomRoleBase customRole) : CustomRoleEventArgs(customRole), IPlayerEvent
{
	/// <summary>
	/// Gets the player calling the event.
	/// </summary>
	public Player? Player { get; } = player;
}
