using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[HarmonyPatch]
internal static class SelfSwallowHasTurnEndInHandEffectOnDiscardPatches
{
    private static readonly object SelfSwallowNotifyQueueLock = new object();
    private static Task SelfSwallowNotifyQueue = Task.CompletedTask;
    private static bool HasSelfSwallow(CardModel card) => card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow);
    private static bool IsEndOfTurnPhase()
    {
        return CombatManager.Instance?.EndingPlayerTurnPhaseOne == true || CombatManager.Instance?.EndingPlayerTurnPhaseTwo == true;
    }
    private static void EnqueueSelfSwallowNotify(CombatState combatState, CardModel card)
	{
		lock (SelfSwallowNotifyQueueLock)
		{
			// 串行化消逝触发，避免多张牌同帧并发导致依赖计数的遗物（如 JozzPaper）重复结算。
			SelfSwallowNotifyQueue = SelfSwallowNotifyQueue.ContinueWith(
				_ => NotifySelfSwallow(combatState, card),
				TaskScheduler.Default).Unwrap();
		}
	}
    private static async Task NotifySelfSwallow(CombatState combatState, CardModel card)
    {
        CombatManager.Instance.History.CardExhausted(combatState, card);
        CombatManager.Instance.History.CardSwallowed(combatState, card);
        await TheInsatiableHook.AfterCardSwallow(combatState, new BlockingPlayerChoiceContext(), card, causedBySelfSwallow: true);
		await Hook.AfterCardExhausted(combatState, new BlockingPlayerChoiceContext(), card, causedByEthereal: false);
    }

    /// <summary>
    /// 单卡 Add 拦截
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(
        typeof(CardPileCmd),
        nameof(CardPileCmd.Add),
        [
            typeof(CardModel),
            typeof(CardPile),
            typeof(CardPilePosition),
            typeof(AbstractModel),
            typeof(bool),
        ])]
    private static void AddSingleToDiscard_RedirectFadeToExhaust(CardModel card, ref CardPile newPile)
    {
        if (newPile.Type != PileType.Discard || !HasSelfSwallow(card))
        {
            return;
        }
        if (!card.HasTurnEndInHandEffect)
        {
            return;
        }

        // 【新增】：如果不是回合结束阶段，则不触发吞噬，正常进入弃牌堆
        if (!IsEndOfTurnPhase())
        {
            return;
        }

        Player? owner = card.Owner;
        if (owner == null)
        {
            return;
        }

        CardPile? SelfSwallow = CardPile.Get(PileType.Exhaust, owner);
        if (SelfSwallow == null)
        {
            return;
        }
		SelfSwallowTracker.MarkPendingSelfSwallowNotify(card);
        newPile = SelfSwallow;
    }

    /// <summary>
    /// 批量/内部 AddInternal 拦截
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CardPile), nameof(CardPile.AddInternal))]
    private static bool AddInternal_Prefix(CardPile __instance, CardModel card, int index, bool silent)
    {
        if (__instance.Type != PileType.Discard || !HasSelfSwallow(card))
        {
            return true;
        }
        if (!card.HasTurnEndInHandEffect)
        {
            return true;
        }

        // 【新增】：如果不是回合结束阶段，则放行 (返回 true)，不触发吞噬
        if (!IsEndOfTurnPhase())
        {
            return true;
        }

        Player? owner = card.Owner;
        if (owner == null)
        {
            return true;
        }

        CardPile? SelfSwallow = CardPile.Get(PileType.Exhaust, owner);
        if (SelfSwallow == null)
        {
            return true;
        }
		SelfSwallowTracker.MarkPendingSelfSwallowNotify(card);
        SelfSwallow.AddInternal(card, index, silent);
        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CardPile), nameof(CardPile.AddInternal))]
    private static void AddInternal_Postfix(CardPile __instance, CardModel card)
    {
        if (__instance.Type != PileType.Exhaust || !SelfSwallowTracker.ConsumePendingSelfSwallowNotify(card))
        {
            return;
        }
        if (!card.HasTurnEndInHandEffect)
        {
            return;
        }
        CombatState? combatState = (CombatState?)(card.CombatState ?? card.Owner?.Creature.CombatState);
        if (combatState == null || CombatManager.Instance == null)
        {
            return;
        }
        EnqueueSelfSwallowNotify(combatState, card);
    }

    /// <summary>
    /// 拦截弃牌历史记录
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CombatHistory), nameof(CombatHistory.CardDiscarded))]
    private static bool CardDiscarded_Prefix(CombatState combatState, CardModel card)
    {
        if (!HasSelfSwallow(card)) return true;
        // 【修改】：只有在回合结束阶段被吞噬时才拦截记录；非回合结束正常弃牌需记录
        return !IsEndOfTurnPhase(); 
    }

    /// <summary>
    /// 拦截弃牌 Hook 触发
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Hook), nameof(Hook.AfterCardDiscarded))]
    private static bool AfterCardDiscarded_Prefix(CombatState combatState, PlayerChoiceContext choiceContext, CardModel card)
    {
        if (!HasSelfSwallow(card)) return true;
        // 【修改】：只有在回合结束阶段被吞噬时才拦截 Hook；非回合结束正常弃牌需触发 Hook
        return !IsEndOfTurnPhase();
    }
}
