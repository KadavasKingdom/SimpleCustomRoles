using System.ComponentModel;

namespace SimpleCustomRoles.RoleYaml;

public class ExtraInfo
{
	[Description("Enable Door Bypassing.")]
	public bool Bypass { get; set; }

	[Description("Open all doors next to spawned place.")]
	public bool OpenDoorsNextToSpawn { get; set; }

	public bool Trigger096 { get; set; } = true;

	public bool Observe173 { get; set; } = true;

	public bool ForceSet { get; set; }

	public bool CannotRevivedByScp049 { get; set; }

	public bool HideFromSpectators { get; set; }

	public bool CanOptOut { get; set; } = true;
}
