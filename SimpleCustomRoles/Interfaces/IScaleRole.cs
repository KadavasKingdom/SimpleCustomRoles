using UnityEngine;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for scaling the role.
/// </summary>
public interface IScaleRole
{
	/// <summary>
	/// Gets the scale of the role.
	/// </summary>
	public Vector3 Scale { get; }

	/// <summary>
	/// Gets the fake scale of the role.
	/// </summary>
	public Vector3 FakeScale { get; }
}
