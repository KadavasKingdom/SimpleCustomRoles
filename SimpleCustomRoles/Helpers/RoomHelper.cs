using LabApi.Features.Wrappers;

namespace SimpleCustomRoles.Helpers;

public static class RoomHelper
{
	public static UnityEngine.Vector3 AdjustRoomPosition(this Room room)
	{
		UnityEngine.Vector3 position = room.Position;
		position.y += 1.5f;
		return position;
	}
}
