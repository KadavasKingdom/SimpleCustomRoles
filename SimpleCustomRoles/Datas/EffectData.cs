#pragma warning disable IDE0130 // Namespace does not match folder structure
using CustomPlayerEffects;

namespace SimpleCustomRoles;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Represent an effect data for custom role effects.
/// </summary>
public sealed class EffectData
{
	/// <summary>
	/// Gets or sets a value indicating whether the effect can be healed.
	/// </summary>
	/// <remarks>
	/// Only used when the effect is <see cref="IHealableEffect"/>.
	/// </remarks>
	public bool IsHealable { get; set; } = true;

	/// <summary>
	/// Gets or sets a value indicating whether the effect is immune to the role.
	/// </summary>
	public bool Immune { get; set; }

	/// <summary>
	/// Gets or sets the effect name.
	/// </summary>
	public string EffectName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the duration of the effect.
	/// </summary>
	public float Duration { get; set; }

	/// <summary>
	/// Gets or sets the intensity of the effect.
	/// </summary>
	public byte Intensity { get; set; }
}
