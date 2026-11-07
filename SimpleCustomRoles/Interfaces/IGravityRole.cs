using UnityEngine;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for changing gravity for the role.
/// </summary>
public interface IGravityRole
{
	/// <summary>
	/// Gets the role gravity.
	/// </summary>
	public Vector3 Gravity { get; }
}
