namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for stamina stats.
/// </summary>
public interface IStaminaStatRole
{
	/// <summary>
	/// Gets the math value for max stamina.
	/// </summary>
	public MathValueFloat MaxStamina { get; }
}
