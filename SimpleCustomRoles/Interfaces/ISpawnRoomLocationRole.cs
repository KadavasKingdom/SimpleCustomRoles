using MapGeneration;

namespace SimpleCustomRoles.Interfaces;

/// <summary>
/// An interface for spawn on room.
/// </summary>
public interface ISpawnRoomLocationRole
{
	/// <summary>
	/// Gets the rooms the role can spawn in.
	/// </summary>
	public List<RoomName> SpawnRooms { get; }
}
