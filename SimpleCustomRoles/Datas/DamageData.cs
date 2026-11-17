namespace SimpleCustomRoles.Datas;

/// <summary>
/// Reprsent a data for custom damage with math value.
/// </summary>
/// <param name="Type">The type of damage.</param>
/// <param name="SubType">The subtype of damage if exists.</param>
/// <param name="SubValue">The subtype value to change.</param>
public record class DamageData(DamageType Type, DamageSubType SubType, object? SubValue) : MathValueFloat
{
	/// <summary>
	/// Gets an empty damage data.
	/// </summary>
	public static DamageData Empty { get; } = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="DamageData"/> class.
	/// </summary>
	public DamageData()
		: this(DamageType.None, DamageSubType.None, null)
	{
	}
}
