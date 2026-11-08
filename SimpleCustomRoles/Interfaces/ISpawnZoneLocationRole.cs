using MapGeneration;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for spawn on zone.
/// </summary>
public interface ISpawnZoneLocationRole
{
	/// <summary>
	/// Gets the zones the role can spawn in.
	/// </summary>
	public List<FacilityZone> SpawnZones { get; }

	/// <summary>
	/// Gets the rooms to exlcude when the role spawns.
	/// </summary>
	public List<RoomName> SpawnZoneExcludeRooms { get; }
}
