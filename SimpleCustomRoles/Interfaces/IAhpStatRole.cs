namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for ahp stats.
/// </summary>
public interface IAhpStatRole
{
	/// <summary>
	/// Gets the math value for ahp.
	/// </summary>
	public MathValueFloat Ahp { get; }

	/// <summary>
	/// Gets the math value for max ahp.
	/// </summary>
	public MathValueFloat MaxAhp { get; }
}
