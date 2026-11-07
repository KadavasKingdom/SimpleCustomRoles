using SimpleCustomRoles.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Extensions;

#pragma warning disable CA1034 // Nested types should not be visible

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
	public static void Apply([NotNull] IAppearanceRole role, [NotNull] Player player)
	{
		if (role.Appearance == PlayerRoles.RoleTypeId.None)
		{
			return;
		}

		player.AddFakeRole(role.Appearance);
	}
}
