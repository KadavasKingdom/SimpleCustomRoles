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
        // Selecting ideal escaping.
        if (Main.Instance.Config.EscapeConfigs.Count == 0)
            return;

        if (ev.EscapeScenario is not Escape.EscapeScenarioType.None)
            return;

        var potentialEscapeRoles = Main.Instance.Config.EscapeConfigs.Where(
            x => 
            x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && 
            x.Key.EscapeRole == ev.Player.Role)
            .ToList();

        if (potentialEscapeRoles.Count == 0)
            return;

        var roleTypeToEscapeTo = potentialEscapeRoles.Select(static x => x.Value).FirstOrDefault();
        if (roleTypeToEscapeTo == PlayerRoles.RoleTypeId.None)
            return;

        // Hanlding escaping
        EscapeDrop(ev.Player);

        ev.IsAllowed = true;
        ev.NewRole = roleTypeToEscapeTo;
        ev.EscapeScenario = Escape.EscapeScenarioType.Custom;
        PlayerEscaped.Add(ev.Player);
        Timing.CallDelayed(1.5f, 
            () => PlayerEscaped.Remove(ev.Player));
    }



    public void EscapeAsCustomRole(PlayerEscapingEventArgs ev, CustomRoleBaseInfo customRole)
    {
        
        if (!customRole.Escape.CanEscape)
        {
            ev.IsAllowed = false;
            return;
        }

        var potentialCustomEscapeRoles = customRole.Escape.ConfigToRole.Where(
            x => 
            x.Key.ShouldBeCuffer == ev.Player.IsDisarmed && 
            x.Key.EscapeRole == ev.Player.Role)
            .ToList();

        if (potentialCustomEscapeRoles.Count == 0)
            return;

        var roleToEscapeTo = potentialCustomEscapeRoles.Select(static x => x.Value).FirstOrDefault();
        if (roleToEscapeTo == default)
            return;

        ev.IsAllowed = false;

        bool RunOriginal = true;
        Events.TriggerOnEscaping(ev.Player, customRole, ref RunOriginal);
        if (!RunOriginal)
        {
            return;
        }

        EscapeDrop(ev.Player);

        CustomRoleHelpers.SetNewRole(ev.Player, roleToEscapeTo, true);
        PlayerEscaped.Add(ev.Player);
        Timing.CallDelayed(1.5f, () => PlayerEscaped.Remove(ev.Player));
    }
}
