using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts;

[HarmonyPatch]
internal static class SelfSwallowHasSelfSwallowOnDiscardPatches
{
	private static readonly object SelfSwallowNotifyQueueLock = new object();
	private static Task SelfSwallowNotifyQueue = Task.CompletedTask;

	private static bool HasSelfSwallow(CardModel card) => card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow);

	private static void EnqueueSelfSwallowHasSelfSwallowSelfSwallowNotify(CombatState combatState, CardModel card)
	{
		lock (SelfSwallowNotifyQueueLock)
		{
			// 串行化消逝触发，避免多张牌同帧并发导致依赖计数的遗物（如 JozzPaper）重复结算。
			SelfSwallowNotifyQueue = SelfSwallowNotifyQueue.ContinueWith(
				_ => NotifySelfSwallowHasSelfSwallowSelfSwallowed(combatState, card),
				TaskScheduler.Default).Unwrap();
		}
	}

	private static async Task NotifySelfSwallowHasSelfSwallowSelfSwallowed(CombatState combatState, CardModel card)
	{
		CombatManager.Instance.History.Entries.OfType<CardSwallowedEntry>();
		await TheInsatiableHook.AfterCardSwallow(combatState, new BlockingPlayerChoiceContext(), card, causedBySelfSwallow: true);
	}

	/// <summary>
	/// 单卡 <see cref="CardPileCmd.Add(CardModel, CardPile, CardPilePosition, AbstractModel?, bool)"/>（含 <c>Add(card, PileType.Discard)</c>、<see cref="CardCmd.Discard"/> 的逐张弃牌）
	/// 在入口把目标堆改为消耗堆，使 <see cref="CardPileCmd"/> 内动画与 <see cref="CardModel.Pile"/> 解析一致。
	/// 批量 <c>Add(IEnumerable, discardPile)</c> 仍走下方 <see cref="CardPile.AddInternal"/> 前缀兜底。
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
	private static void AddSingleToDiscard_RedirectSelfSwallowHasSelfSwallowToSelfSwallow(CardModel card, ref CardPile newPile)
	{
		if (newPile.Type != PileType.Discard || !HasSelfSwallow(card))
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
		newPile = SelfSwallow;
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(CardPile), nameof(CardPile.AddInternal))]
	private static bool AddInternal_Prefix(CardPile __instance, CardModel card, int index, bool silent)
	{
		if (__instance.Type != PileType.Discard || !HasSelfSwallow(card))
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
		SelfSwallow.AddInternal(card, index, silent);
		return false;
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(CardPile), nameof(CardPile.AddInternal))]
	private static void AddInternal_Postfix(CardPile __instance, CardModel card)
	{
		if (__instance.Type != PileType.Exhaust)
		{
			return;
		}

		CombatState? combatState = (CombatState?)(card.CombatState ?? card.Owner?.Creature.CombatState);
		if (combatState == null || CombatManager.Instance == null)
		{
			return;
		}

		EnqueueSelfSwallowHasSelfSwallowSelfSwallowNotify(combatState, card);
	}

	[HarmonyPrefix]
	[HarmonyPatch(typeof(CombatHistory), nameof(CombatHistory.CardDiscarded))]
	private static bool CardDiscarded_Prefix(CombatState combatState, CardModel card) => !HasSelfSwallow(card);

	[HarmonyPrefix]
	[HarmonyPatch(typeof(Hook), nameof(Hook.AfterCardDiscarded))]
	private static bool AfterCardDiscarded_Prefix(CombatState combatState, PlayerChoiceContext choiceContext, CardModel card) => !HasSelfSwallow(card);
}
