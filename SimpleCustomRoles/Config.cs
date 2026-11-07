namespace SimpleCustomRoles;

/// <summary>
/// Represent a custom roles config.
/// </summary>
public sealed class Config
{
	/// <summary>
	/// Gets or sets a value indicating whether debug is enabled.
	/// </summary>
	public bool Debug { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether to use global folder for loading roles..
	/// </summary>
	public bool UseGlobal { get; set; }
}
