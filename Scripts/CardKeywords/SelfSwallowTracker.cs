using System.Collections.Generic;
using MegaCrit.Sts2.Core.Models;


namespace TheInsatiable.Scripts;

/// <summary>
/// 追踪「本应进弃牌堆、经 Patch 改入消耗堆」的牌，用于补发 <see cref="MegaCrit.Sts2.Core.Combat.History.CombatHistory.CardExhausted"/> 与 <see cref="MegaCrit.Sts2.Core.Hooks.Hook.AfterCardExhausted"/>。
/// </summary>
internal static class SelfSwallowTracker
{
	private static readonly HashSet<CardModel> _pendingSelfSwallowNotify = new();

	internal static void MarkPendingSelfSwallowNotify(CardModel card) => _pendingSelfSwallowNotify.Add(card);

	internal static bool ConsumePendingSelfSwallowNotify(CardModel card) => _pendingSelfSwallowNotify.Remove(card);
}