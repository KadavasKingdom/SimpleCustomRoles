using PlayerRoles.PlayableScps.HumeShield;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for humeshield stats.
/// </summary>
public interface IHumeShieldStatRole
{
	/// <summary>
	/// Gets the math value for hume shield regeneration rate.
	/// </summary>
	public MathValueFloat HumeShieldRegenRate { get; }

	/// <summary>
	/// Gets the math value for hume shield regeneration cooldown.
	/// </summary>
	public MathValueFloat HumeShieldRegenCooldown { get; }

	/// <summary>
	/// Gets a dictionary for controlling <see cref="DynamicHumeShieldController.ShieldOverHealth"/>.
	/// </summary>
	public Dictionary<float, float> ShieldOverHealth { get; }

	/// <summary>
	/// Gets a value indicating whether to clear <see cref="DynamicHumeShieldController.ShieldOverHealth"/>.
	/// </summary>
	public bool ClearShieldOverHealth { get;  }
}
