using LabApi.Features.Wrappers;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCustomRoles.Helpers;

public static class RoomHelper
{
	public static UnityEngine.Vector3 AdjustRoomPosition([NotNull] this Room room)
	{
		UnityEngine.Vector3 position = room.Position;
		position.y += 1.5f;
		return position;
	}
}
