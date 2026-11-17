using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerStatsSystem;
using SimpleCustomRoles.Datas;
using SimpleCustomRoles.Interfaces;
using SimpleCustomRoles.Roles;

namespace SimpleCustomRoles.EventHandlers;

/// <summary>
/// Represent a hurt event handler.
/// </summary>
internal sealed class HurtHandler : CustomEventsHandler
{
	private static void DamageCalculation(ref DamageHandlerBase handlerBase, List<DamageData> datas)
	{
		float damage = handlerBase.GetDamageValue();
		DamageType damageType = handlerBase.GetDamageType();

		foreach (DamageData data in datas)
		{
			if (data.Type is DamageType.None || data.Math is MathOption.None)
			{
				continue;
			}

			if (data.Type is DamageType.Any)
			{
				data.MathCalculation(ref damage);
				continue;
			}

			if (data.Type != damageType)
			{
				continue;
			}

			if (data.SubType is not DamageSubType.None)
			{
				object? value = handlerBase.GetObjectBySubType(data.SubType);

				if (value != data.SubValue)
				{
					continue;
				}
			}

			data.MathCalculation(ref damage);
		}

		handlerBase.SetDamageValue(damage);
	}

	/// <inheritdoc/>
	public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
	{
		DamageHandlerBase handler = ev.DamageHandler;
		if (CustomRoles.TryGetCustomRole(ev.Attacker, out CustomRoleBase? attackerRole) && attackerRole is IDamageDealChangeRole damageDealChange)
		{
			DamageCalculation(ref handler, damageDealChange.DamageDealt);
		}

		if (CustomRoles.TryGetCustomRole(ev.Player, out CustomRoleBase? playerRole) && playerRole is IDamageReceiveChangeRole damageReceiveChange)
		{
			DamageCalculation(ref handler, damageReceiveChange.DamageReceived);
		}

		ev.DamageHandler = handler;
	}
}
