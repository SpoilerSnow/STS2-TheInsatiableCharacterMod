using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts;

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

            foreach (var card in toRemove)
            {
                await CardPileCmd.RemoveFromCombat(card, skipVisuals: false);
                SwallowPile.CardTurns.Remove(card);
            }
        }
    }
}