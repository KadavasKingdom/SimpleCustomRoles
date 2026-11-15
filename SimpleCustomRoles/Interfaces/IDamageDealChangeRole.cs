using SimpleCustomRoles.Datas;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for changing the damages the player dealt to the role.
/// </summary>
public interface IDamageDealChangeRole
{
	/// <summary>
	/// Gets the list of damages to change.
	/// </summary>
	public List<DamageData> DamageDealt { get; }
}
