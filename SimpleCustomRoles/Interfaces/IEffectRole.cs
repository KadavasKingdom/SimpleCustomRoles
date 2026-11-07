namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// an interface for providing effects for the role.
/// </summary>
public interface IEffectRole
{
	/// <summary>
	/// Gets the effects.
	/// </summary>
	public List<EffectData> Effects { get; }
}
