using System.Reflection;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Interop;

namespace TheInsatiable.Scripts;

[ModInitializer("Init")]
public class Entry
{
	public static void Init()
	{
		ModTypeDiscoveryHub.RegisterModAssembly("TheInsatiable", Assembly.GetExecutingAssembly());
		RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<SandBlowing, SandStorm>();
		RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<DesertStoneRelic, PolishedDesertStoneRelic>();
		RitsuLibFramework.CreateContentPack("TheInsatiable")
			.Card<InsatiableCardPool, InsatiableCardModel>()
			.Relic<InsatiableRelicPool, InsatiableRelicModel>()
			.Potion<InsatiablePotionPool, InsatiablePotionModel>()
			.Apply();

		var harmony = new Harmony("sts2.spoilersnow.theinsatiablemod");
		harmony.PatchAll();
		ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
		Log.Debug("Mod initialized!");
	}
}
