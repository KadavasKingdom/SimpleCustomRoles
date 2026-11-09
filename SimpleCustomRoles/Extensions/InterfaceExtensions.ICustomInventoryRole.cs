using SimpleCustomRoles.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Extensions;

#pragma warning disable CA1305 // Specify IFormatProvider

/// <summary>
/// Provides extension for Custom Roles Interfaces.
/// </summary>
public static partial class InterfaceExtensions
{
	/// <summary>
	/// Applies <paramref name="role"/> to the <paramref name="player"/>.
	/// </summary>
	/// <param name="role">The role.</param>
	/// <param name="player">The player.</param>
	public static void Apply([NotNull] this ICustomInventoryRole role, [NotNull] Player player)
	{
		foreach (var item in role.CustomNames)
		{
			Server.RunCommand(string.Format(Main.Instance.Config.CustomItemCommand, item, player.PlayerId));
		}

		foreach (var item in role.CustomIds)
		{
			Server.RunCommand(string.Format(Main.Instance.Config.CustomItemCommand, item, player.PlayerId));
		}
	}
}
