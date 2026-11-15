using SimpleCustomRoles.Datas;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for changing the damages the player received to the role.
/// </summary>
public interface IDamageReceiveChangeRole
{
	/// <summary>
	/// Gets the list of damages to change.
	/// </summary>
	public List<DamageData> DamageReceived { get; }
}
