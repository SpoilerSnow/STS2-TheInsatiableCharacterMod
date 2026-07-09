using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Piles;

/// <summary>
/// 吞噬堆逻辑管理：最大容量 10 张，存满 3 回合的卡牌自动消失。
/// </summary>
internal static class SwallowPile
{
    public const int DefaultMaxCapacity = 10;
    public static int MaxCapacity = DefaultMaxCapacity;
    public const int DefaultMaxTurnsInPile = 4;
    public static int MaxTurnsInPile = DefaultMaxTurnsInPile;

    /// <summary>
    /// 追踪每张卡牌在吞噬堆中度过的回合数。
    /// </summary>
    internal static readonly Dictionary<CardModel, int> CardTurns = new();

    /// <summary>
    /// 卡牌被吞噬时调用，初始化回合计数。
    /// </summary>
    internal static void OnCardAdded(CardModel card)
    {
        CardTurns[card] = 0;
    }

    /// <summary>
    /// 清理不再存在于吞噬堆的卡牌条目（已被其他方式移除的卡牌）。
    /// </summary>
    internal static void CleanStaleEntries(ICollection<CardModel> currentCards)
    {
        var stale = new List<CardModel>();
        foreach (var card in CardTurns.Keys)
        {
            if (!currentCards.Contains(card))
                stale.Add(card);
        }
        foreach (var card in stale)
            CardTurns.Remove(card);
    }
    /// <summary>
    /// 战斗结束时重置吞噬堆状态：恢复默认容量上限并清空回合计数。
    /// </summary>
    internal static void ResetForNewCombat()
    {
        MaxCapacity = DefaultMaxCapacity;
        CardTurns.Clear();
        MaxTurnsInPile = DefaultMaxTurnsInPile;
    }
}

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

[HarmonyPatch(typeof(CombatManager), "CombatEnded")]
internal static class SwallowPileResetPatch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        SwallowPile.ResetForNewCombat();
    }
}
