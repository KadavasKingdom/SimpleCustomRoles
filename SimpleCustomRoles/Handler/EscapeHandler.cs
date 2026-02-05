using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using MEC;
using NorthwoodLib.Pools;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Handler;

public class EscapeHandler : CustomEventsHandler
{
    public static HashSet<Player> PlayerEscaped = [];
    public static HashSet<Player> NeverEscape = [];

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (ev.Player != null)
            NeverEscape.Remove(ev.Player);
    }

    public override void OnPlayerEscaping(PlayerEscapingEventArgs ev)
    {
        Player player = ev.Player;

        if (NeverEscape.Contains(player))
            return;

        if (PlayerEscaped.Contains(player))
            return;
        
        if (!CustomRoleHelpers.TryGetCustomRole(ev.Player, out var customRole))
        {
            EscapeAsNoRole(ev);
        }
        else
        {
            CL.Debug($"Escaping {ev.Player.PlayerId}", Main.Instance.Config.DebugEscape);
            EscapeAsCustomRole(ev , customRole);
        }
    }

    public void EscapeDrop(Player player)
    {
        List<Pickup> droppedItems = ListPool<Pickup>.Shared.Rent();
        foreach (var item in player.Items.ToList())
        {
            if (item is Scp1344Item scp1344Item)
                scp1344Item.Status = InventorySystem.Items.Usables.Scp1344.Scp1344Status.Idle;
            var dropped = item.DropItem();
            droppedItems.Add(dropped);
        }
        foreach (var item in player.Ammo.ToList())
        {
            droppedItems.AddRange(player.DropAmmo(item.Key, item.Value));
        }
        Timing.CallDelayed(2.5f, () =>
        {
            foreach (var item in droppedItems)
            {
                item.Position = player.Position;
            }
            ListPool<Pickup>.Shared.Return(droppedItems);
        });
    }

    public void EscapeAsNoRole(PlayerEscapingEventArgs ev)
    {
        if (ev.EscapeScenario is not Escape.EscapeScenarioType.None or Escape.EscapeScenarioType.Custom)
        {
            return;
        }

        // Selecting ideal escaping.
        if (Main.Instance.Config.EscapeConfigs.Count == 0)
        {
            NeverEscape.Add(ev.Player);
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} DENYCHECK1");
                CL.Debug("Config doesnt have config option!.");
            }
            return;
        }

        var potentialEscapeRoles = Main.Instance.Config.EscapeConfigs.Where(
            x => 
            x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && 
            x.Key.EscapeRole == ev.Player.Role)
            .ToList();

        if (potentialEscapeRoles.Count == 0)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} DENYCHECK3");
                CL.Debug($"[{ev.Player.PlayerId}] Potential escape role is 0! {ev.Player.Role} {ev.Player.IsDisarmed}.");
            }

            if (!Main.Instance.Config.EscapeConfigs.Any(x => x.Key.EscapeRole == ev.Player.Role))
            {
                // Even if can escape we dont have a config for this role.
                NeverEscape.Add(ev.Player);
            }
            return;
        }

        var roleTypeToEscapeTo = potentialEscapeRoles.Select(static x => x.Value).FirstOrDefault();
        if (roleTypeToEscapeTo == PlayerRoles.RoleTypeId.None)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} DENYCHECK4");
                CL.Debug($"[{ev.Player.PlayerId}] roleTypeToEscapeTo is None! {ev.Player.Role} {ev.Player.IsDisarmed} {roleTypeToEscapeTo}.", Main.Instance.Config.DebugEscape);
            }
            return;
        }

        // Hanlding escaping
        EscapeDrop(ev.Player);

        ev.IsAllowed = true;
        ev.NewRole = roleTypeToEscapeTo;
        ev.EscapeScenario = Escape.EscapeScenarioType.Custom;
        PlayerEscaped.Add(ev.Player);
        Timing.CallDelayed(1.5f, 
            () => PlayerEscaped.Remove(ev.Player));
        ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} SUCCESS");
        CL.Debug($"[{ev.Player.PlayerId}] is escaped as: {roleTypeToEscapeTo}.", Main.Instance.Config.DebugEscape);
    }



    public void EscapeAsCustomRole(PlayerEscapingEventArgs ev, CustomRoleBaseInfo customRole)
    {
        CL.Debug($"[{ev.Player.PlayerId}] Escaping as custom role {customRole.Rolename}.", Main.Instance.Config.DebugEscape);
        if (!customRole.Escape.CanEscape)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} DENYCHECK1");
                CL.Debug($"[{ev.Player.PlayerId}] Escaping as custom role {customRole.Rolename} set to Cannot Escape!", Main.Instance.Config.DebugEscape);
            }
            ev.IsAllowed = false;
            return;
        }

        var potentialCustomEscapeRoles = customRole.Escape.ConfigToRole.Where(
            x => 
            x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && 
            x.Key.EscapeRole == ev.Player.Role)
            .ToList();

        if (potentialCustomEscapeRoles.Count == 0)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now:HH:mm:ss.fff} DENYCHECK2");
                CL.Debug($"[{ev.Player.PlayerId}] Potential escape role is 0! {ev.Player.Role} {ev.Player.IsDisarmed}.", Main.Instance.Config.DebugEscape);
            }

            if (!customRole.Escape.ConfigToRole.Any(x => x.Key.EscapeRole == ev.Player.Role))
            {
                // Even if can escape we dont have a config for this role.
                NeverEscape.Add(ev.Player);
            }
            return;
        }

        var roleToEscapeTo = potentialCustomEscapeRoles.Select(static x => x.Value).FirstOrDefault();
        if (roleToEscapeTo == default)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now.ToString("HH:mm:ss.fff")} DENYCHECK3");
                CL.Debug($"[{ev.Player.PlayerId}] roleTypeToEscapeTo is None! {ev.Player.Role} {ev.Player.IsDisarmed} {roleToEscapeTo}.", Main.Instance.Config.DebugEscape);
            }
            return;
        }

        ev.IsAllowed = false;

        bool RunOriginal = true;
        Events.TriggerOnEscaping(ev.Player, customRole, ref RunOriginal);
        if (!RunOriginal)
        {
            if (Main.Instance.Config.DebugEscape)
            {
                ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now.ToString("HH:mm:ss.fff")} DENY3RDPARTY");
                CL.Debug($"[{ev.Player.PlayerId}] Escaping has been denied by 3rd party! {RunOriginal}", Main.Instance.Config.DebugEscape);
            }
            return;
        }

        EscapeDrop(ev.Player);
        if (Main.Instance.Config.DebugEscape)
        {
            CL.Debug($"[{ev.Player.PlayerId}] is escaping!", Main.Instance.Config.DebugEscape);
            ev.Player.SendConsoleMessage($"[ESCAPEDEBUG] {DateTime.Now.ToString("HH:mm:ss.fff")} SUCCESS");
        }
        CustomRoleHelpers.SetNewRole(ev.Player, roleToEscapeTo, true); 
        PlayerEscaped.Add(ev.Player);
        Timing.CallDelayed(1.5f, () => PlayerEscaped.Remove(ev.Player));
    }
}
