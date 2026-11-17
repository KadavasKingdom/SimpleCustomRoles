using PlayerRoles;

namespace SimpleCustomRoles.Datas;

/// <summary>
/// Represent an escape data record.
/// </summary>
/// <param name="IsCuffed">Whenever the escape person should be cuffed.</param>
/// <param name="EscapeRole">The escaper role.</param>
public record struct EscapeData(bool IsCuffed, RoleTypeId EscapeRole)
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EscapeData"/> struct.
	/// </summary>
	public EscapeData()
		: this(false, RoleTypeId.None)
	{
	}
}
