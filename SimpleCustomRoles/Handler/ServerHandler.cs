using LabApi.Events.CustomHandlers;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;
using System.Linq;

namespace SimpleCustomRoles.Handler;

internal class ServerHandler : CustomEventsHandler
{
	public static void ReloadRoles()
	{
		Main.Instance.InWaveRoles = [];
		Main.Instance.ScpSpecificRoles = [];
		RolesLoader.Load();
		foreach (var item in RolesLoader.RoleInfos)
		{
			if (item.RoleType == CustomRoleType.AfterDead)
			{
				continue;
			}

			for (int i = 0; i < item.Spawn.SpawnAmount; i++)
			{
				if (item.RoleType == CustomRoleType.ScpSpecific)
				{
					Main.Instance.ScpSpecificRoles.Add(item);
				}

				if (item.RoleType == CustomRoleType.InWave)
				{
					Main.Instance.InWaveRoles.Add(item);
				}
			}
		}
	}

	public override void OnServerWaitingForPlayers()
	{
		Main.Instance.InWaveRoles = [];
		Main.Instance.ScpSpecificRoles = [];
		Main.Instance.EscapeRoles = [];
		Main.Instance.RiggedRoles = [];
		RolesLoader.Load();
		CL.Info("Loaded custom roles!");
	}

	public override void OnServerRoundStarted()
	{
		if (Main.Instance.Config.IsPaused)
		{
			return;
		}

		// Round lock for certain roles.
		if (!Round.IsLocked)
		{
			Round.IsLocked = true;
			Timing.CallDelayed(5, () =>
			{
				Round.IsLocked = false;
			});
		}

		List<CustomRoleBaseInfo> regularRoles = [];
		foreach (var item in RolesLoader.RoleInfos)
		{
			if (item.RoleType == CustomRoleType.AfterDead)
			{
				continue;
			}

			for (int i = 0; i < item.Spawn.SpawnAmount; i++)
			{
				bool isSpawning = false;
				var random = URandom.Range(1, 10000);
				if (!CustomRoleHelpers.IsShouldSpawn(item))
				{
					CL.Debug($"Role has been no longer spawn: {item.Rolename} (Reason: Player limited)", Main.Instance.Config.Debug);
					continue;
				}

				if (!GroupHelper.CanSpawn(item.Rolegroup, ref regularRoles))
				{
					CL.Debug($"Role has been no longer spawn: {item.Rolename} (Reason: Group limited)", Main.Instance.Config.Debug);
					break;
				}

				GroupHelper.DenyCustomRoles(item.Rolegroup, ref regularRoles);
				int chance = item.Spawn.SpawnChance;
				if (Main.Instance.Config.UsePlayerPercent && !item.Spawn.DenyChance)
				{
					CL.Debug($"Basic chance: {chance}", Main.Instance.Config.Debug);
					float chance_mulitplier = (float)Server.PlayerCount / (float)Server.MaxPlayers;
					chance = (int)(chance * chance_mulitplier);
					CL.Debug($"Final chance: {chance}", Main.Instance.Config.Debug);
				}

				if (!item.Spawn.DenyChance)
				{
					chance = (int)((float)chance * Main.Instance.Config.SpawnRateMultiplier);
				}

				if (random <= chance)
				{
					isSpawning = true;
					if (item.RoleType == CustomRoleType.ScpSpecific)
					{
						Main.Instance.ScpSpecificRoles.Add(item);
					}

					if (item.RoleType == CustomRoleType.InWave)
					{
						Main.Instance.InWaveRoles.Add(item);
					}
					else
					{
						regularRoles.Add(item);
					}
				}

				CL.Debug($"Rolled chance: {random}/{chance} for Role {item.Rolename}. Role is " + (isSpawning ? string.Empty : "NOT ") + "spawning.", Main.Instance.Config.Debug);
			}
		}

		regularRoles.ShuffleListSecure();

		foreach (var item in regularRoles.Concat(Main.Instance.RiggedRoles).Where(static x => !string.IsNullOrEmpty(x.Rolegroup)).ToArray())
		{
			GroupHelper.DenyCustomRoles(item.Rolegroup, ref regularRoles);
		}

		var riggedAndRegularRoles = Main.Instance.RiggedRoles.Concat(regularRoles).ToList();
		List<Player> notRoll = [];
		List<CustomRoleBaseInfo> rolled = new();
		Timing.CallDelayed(0.2f, () =>
		{
			foreach (var item in riggedAndRegularRoles)
			{
				Player? player = null;

				if (item.ReplaceRole == PlayerRoles.RoleTypeId.None && item.ReplaceTeam != PlayerRoles.Team.Dead)
				{
					var list = Player.ReadyList.Where(x => x.Team == item.ReplaceTeam && !CustomRoleHelpers.Contains(x) && !notRoll.Contains(x)).ToList();
					if (list.Count > 0)
					{
						player = list.RandomItem();
					}
				}
				else
				{
					var list = Player.ReadyList.Where(x => x.Role == item.ReplaceRole && !CustomRoleHelpers.Contains(x) && !notRoll.Contains(x)).ToList();
					if (list.Count > 0)
					{
						player = list.RandomItem();
					}
				}

				if (player == null)
				{
					continue;
				}

				if (Main.Instance.Config.ShowSpawnMessage)
				{
					CL.Info($"Player Selected to spawn: {player.UserId} as {item.Rolename}");
				}

				rolled.Add(item);
				notRoll.Add(player);
				CustomRoleHelpers.SetCustomInfoToPlayer(player, item);
			}

			riggedAndRegularRoles.Clear();
			regularRoles.Clear();

			foreach (var role in Main.Instance.RiggedRoles.Where(x => !rolled.Contains(x)))
			{
				// in case for some reason the other player just didn't get selected
				// if the role replaces an 096 or 079 it's best to get the respective other one (so you dont have overlord on the same team as 096 for example)
				var otherRole = role.ReplaceRole switch
				{
					RoleTypeId.Scp079 => RoleTypeId.Scp096,
					RoleTypeId.Scp096 => RoleTypeId.Scp079,
					_ => RoleTypeId.None,
				};
				var plr = Player.ReadyList.Where(x => x.Role == otherRole).FirstOrDefault();

				// if role replaces a scp, get random scp, otherwise get random human
				plr ??= Player.ReadyList.Where(x => !(role.ReplaceRole.IsScp() ^ x.IsSCP)).GetRandom();
				if (plr == null)
				{
					CL.Error("couldn't find suitable player to replace rigged SCP role");
					continue;
				}

				Timing.CallDelayed(1f, () =>
				{
					CustomRoleHelpers.SetFromCMD(plr, role);
				});
			}

			Main.Instance.RiggedRoles.Clear();
		});
	}
}
