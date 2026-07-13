using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Patches;

[HarmonyPatch(typeof(CombatManager), "DoTurnEnd")]
public static class SwallowPileAgePatch
{
    [HarmonyPostfix]
    public static async Task Postfix(Task __result)
    {
        await __result;
        var players = CombatManager.Instance.DebugOnlyGetState()?.Players;
        if (players == null)
        return;
        foreach (var player in players)
        {
            var pile = Entry.SwallowPile.GetPile(player);
            var cards = pile.Cards.ToList();

            SwallowPile.CleanStaleEntries(cards);

            var toRemove = new List<CardModel>();
            foreach (var card in cards)
            {
                if (!SwallowPile.CardTurns.ContainsKey(card))
                    continue;

                SwallowPile.CardTurns[card]++;

                if (SwallowPile.CardTurns[card] >= SwallowPile.MaxTurnsInPile)
                    toRemove.Add(card);
            }
            if (toRemove.Count != 0)
            {
                await CreatureCmd.TriggerAnim(player.Creature, "Salivate", 0.5f);
            }
            foreach (var card in toRemove)
            {
                await CardPileCmd.Add(card, PileType.Play);
			    await Cmd.Wait(0.2f);
			    NCard nCard = NCard.FindOnTable(card);
                NCardFlyPowerVfx nCardFlyPowerVfx = NCardFlyPowerVfx.Create(nCard);
			    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nCardFlyPowerVfx);
			    TaskHelper.RunSafely(nCardFlyPowerVfx.PlayAnim());
			    float duration = nCardFlyPowerVfx.GetDuration();
			    await Cmd.CustomScaledWait(duration * 0.2f, duration);
                await CardPileCmd.RemoveFromCombat(card, skipVisuals: false);
                SwallowPile.CardTurns.Remove(card);
            }
        }
    }
}