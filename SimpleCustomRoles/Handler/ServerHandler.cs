using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.Pools;
using SimpleCustomRoles.RoleInfo;

namespace SimpleCustomRoles.Handler;

internal class ServerHandler : CustomEventsHandler
{
    public static void ReloadRoles()
    {
        RolesLoader.Load();
        PoolManager.Reset();
    }

    public override void OnServerWaitingForPlayers()
    {
        RolesLoader.Load();
        PoolManager.Reset();
        CL.Info("Loaded custom roles!");
    }

    public override void OnServerRoundStarted()
    {
        // Round lock for certain roles.
        if (!Round.IsLocked)
        {
            Round.IsLocked = true;
            Timing.CallDelayed(5, () =>
            {
                Round.IsLocked = false;
            });
        }

        Timing.CallDelayed(0.2f, () =>
        {
            List<Player> players = [.. Player.ReadyList];
            players.ShuffleListSecure();

            foreach (var player in players)
            {

                if (!player.IsAlive)
                    continue;

                var role = player.GetRandomCustomRoleBaseInfo();

                if (role != null)
                {
                    CustomRoleHelpers.SetCustomInfoToPlayer(player, role);
                } else
                {
                    CL.Debug($"{player.DisplayName} ({player.PlayerId}, {player.Role}) did not roll a custom role", Main.Instance.Config.Debug);
                }
            }
        });
    }
}
