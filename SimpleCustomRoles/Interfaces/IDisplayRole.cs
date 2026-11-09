namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for display role related information.
/// </summary>
public interface IDisplayRole
{
	/// <summary>
	/// Gets a value indicating whether to diplay the role information.
	/// </summary>
	public bool CanDisplay { get; }

	/// <summary>
	/// Gets the name to display.
	/// </summary>
	public string NameDisplay { get; }

	/// <summary>
	/// Gets the name to display on Remote Admin.
	/// </summary>
	public string NameRA { get; }

	/// <summary>
	/// Gets the name to display for spectator.
	/// </summary>
	public string NameSpectator { get; }

	/// <summary>
	/// Gets the color hex value to display.
	/// </summary>
	public string ColorHex { get; }

	/// <summary>
	/// Gets the player info area.
	/// </summary>
	/// <remarks>
	/// When value is <see langword="null"/>, it will not override it.
	/// </remarks>
	public PlayerInfoArea? InfoArea { get; }
}
