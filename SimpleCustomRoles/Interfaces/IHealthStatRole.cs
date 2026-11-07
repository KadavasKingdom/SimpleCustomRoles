namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for health stats.
/// </summary>
public interface IHealthStatRole
{
	/// <summary>
	/// Gets the math value for health.
	/// </summary>
	public MathValueFloat Health { get; }

	/// <summary>
	/// Gets the math value for max health.
	/// </summary>
	public MathValueFloat MaxHealth { get; }
}
