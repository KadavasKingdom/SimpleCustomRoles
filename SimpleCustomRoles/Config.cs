using System.ComponentModel;

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

	/// <summary>
	/// Gets or sets the command to give custom items to player.
	/// </summary>
	[Description("For CustomItemsAPI use \"/lci give {0} {1}\" for exiled use \"/ci give {0} {1}\"")]
	public string CustomItemCommand { get; set; } = "/lci give {0} {1}";
}
