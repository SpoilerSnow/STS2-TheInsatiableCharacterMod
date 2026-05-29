using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace TheInsatiable.Scripts;

[HarmonyPatch(typeof(BigMushroom), "Grow")]
internal static class BigMushroomGrowScalePatch
{
	[HarmonyPrefix]
	private static bool Prefix(BigMushroom __instance)
	{
		NCombatRoom.Instance?.GetCreatureNode(__instance.Owner.Creature)?.ScaleTo(1.2f, 0f);
		return false;
	}
}

[HarmonyPatch(typeof(SurroundedPower))]
internal static class SurroundedPowerInsatiableFacingPatch
{
	[HarmonyPrefix]
	[HarmonyPatch("FlipScale")]
	private static bool FlipScalePrefix(SurroundedPower __instance, Node2D? body, ref Task __result)
	{
		if (__instance.Owner?.Player?.Character is not InsatiableCharacter)
		{
			return true;
		}

		if (body == null)
		{
			__result = Task.CompletedTask;
			return false;
		}

		float x = body.Scale.X;
		SurroundedPower.Direction facing = __instance.Facing;
		bool shouldFlip = (facing == SurroundedPower.Direction.Right && x > 0f)
			|| (facing == SurroundedPower.Direction.Left && x < 0f);
		if (shouldFlip)
		{
			body.Scale = new Vector2(-body.Scale.X, body.Scale.Y);
		}

		__result = Task.CompletedTask;
		return false;
	}
}
