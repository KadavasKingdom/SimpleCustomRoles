using CustomPlayerEffects;
using HarmonyLib;
using SimpleCustomRoles.Interfaces;
using System.Reflection;

namespace SimpleCustomRoles.Patches;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

/// <summary>
/// Patches for <see cref="IHealableEffect.IsHealable(ItemType)"/>.
/// </summary>
[HarmonyPatch]
internal static class IHealableEffect_IsHealable
{
	private static IEnumerable<MethodBase> TargetMethods()
	{
		Type interfaceType = typeof(IHealableEffect);

		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (Type type in assembly.GetTypes()
				.Where(x => x.IsClass && !x.IsAbstract && interfaceType.IsAssignableFrom(x)))
			{
				MethodInfo method = type.GetMethod(nameof(IHealableEffect.IsHealable), BindingFlags.Instance | BindingFlags.Public);
				if (method != null)
				{
					yield return method;
				}
			}
		}
	}

	private static bool Prefix(object __instance, ref bool __result)
	{
		if (__instance is not StatusEffectBase statusEffectBase)
		{
			return true;
		}

		Player player = Player.Get(statusEffectBase.Hub);
		if (player == null)
		{
			return true;
		}

		if (!CustomRoles.TryGetCustomRole(player, out var customRole))
		{
			return true;
		}

		if (customRole is not IEffectRole effectRole)
		{
			return true;
		}

		EffectData? effectData = effectRole.Effects.FirstOrDefault(x => x.EffectName == statusEffectBase.name);
		if (effectData is null || effectData.EffectName != statusEffectBase.name)
		{
			return true;
		}

		__result = effectData.IsHealable;
		return true;
	}
}
