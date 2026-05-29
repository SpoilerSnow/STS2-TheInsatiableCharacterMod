using System;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheInsatiable.Scripts;

[HarmonyPatch(typeof(NCreatureVisuals), "_Ready")]
public class SpineTracker
{
	public static MegaSprite? PlayerSpine { get; private set; }

	private static void Postfix(NCreatureVisuals __instance)
	{
		string text = ((object)((Node)__instance).Name).ToString();
		if (text.Contains("TheInsatiableSpoiler", StringComparison.OrdinalIgnoreCase))
		{
			PlayerSpine = __instance.SpineBody;
		}
	}
}