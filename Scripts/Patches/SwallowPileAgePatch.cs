using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Patches;

/// <summary>
/// 在每回合结束时处理吞噬堆中卡牌的老化，存满 3 回合的卡牌自动消失。
/// </summary>
[HarmonyPatch(typeof(CombatManager), "DoTurnEnd")]
internal static class SwallowPileAgePatch
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
