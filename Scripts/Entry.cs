using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.CardPiles;
using STS2RitsuLib.Interop;
using TheInsatiable.Scripts.Cards;
using TheInsatiable.Scripts.Characters;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Relics;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace TheInsatiable.Scripts;

[ModInitializer("Init")]
public class Entry
{
	public const string ModId = "TheInsatiable";
    public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);
    public static PileType SwallowPile;
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

		var registry = ModCardPileRegistry.For(ModId);
        SwallowPile = registry.RegisterOwned("swallow_pile", new ModCardPileSpec
        {
            Scope = ModCardPileScope.CombatOnly,
            Style = ModCardPileUiStyle.BottomRight,
            Anchor = new ModCardPileAnchor(ModCardPileAnchorKind.BottomRightPrimary, new Vector2(100f, -100f)),
            IconPath = "res://TheInsatiable/images/ui/the_insatiable_energy_big.png",
            OnOpen = ctx => ctx.ShowDefaultPileScreen(),
			VisibleWhen = ctx => ctx.Player != null && ctx.Pile != null && (ctx.Pile.Cards.Any() || ctx.Player.Character is InsatiableCharacter)
        }).PileType;
	}
}
