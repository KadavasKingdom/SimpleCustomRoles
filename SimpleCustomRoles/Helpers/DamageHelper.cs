using LabApiExtensions.Configs;
using LabApiExtensions.Extensions;
using PlayerStatsSystem;
using SimpleCustomRoles.RoleYaml;

namespace SimpleCustomRoles.Helpers;

public static class DamageHelper
{
	// TODO: Make log for this.
	public static float CalculateDamage(this Dictionary<DamageMaker, MathValueFloat> dict, DamageHandlerBase damageHandlerBase, float baseDamage, DamageType damageType)
	{
		float newDamage = baseDamage;
		var damageTypeEnum = dict.Where(x => x.Key.DamageType == damageType).ToList();
		if (damageTypeEnum.Count == 0)
		{
			return newDamage;
		}

		if (damageTypeEnum.Any(x => x.Key.DamageSubType is DamageSubType.None))
		{
			var damageFirst = damageTypeEnum.FirstOrDefault();
			if (damageFirst.Value == default)
			{
				return newDamage;
			}

			return damageFirst.Value.MathCalculation(newDamage);
		}

		foreach (var item in damageTypeEnum.Select(x => x.Key.DamageSubType))
		{
			var obj = damageHandlerBase.GetObjectBySubType(item);
			if (obj == null)
			{
				continue;
			}

			var sent = damageTypeEnum.
				Where(x => x.Key.DamageSubType == item && x.Key.SubType.ToString() == obj.ToString()).ToList();
			if (sent.Count != 0)
			{
				var first = sent.First();
				if (first.Value == null)
				{
					continue;
				}

				newDamage = first.Value.MathCalculation(newDamage);
			}
		}

		return newDamage;
	}
}
