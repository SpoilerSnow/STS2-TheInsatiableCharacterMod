using System.Collections.Generic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

/// <summary>
/// 六英尺下 Power：Amount 对应持续回合数，每回合结束后减一。
/// 单回合内对单个敌人累计施加 6 层流沙后即吞噬，回合结束时重置计数。
/// </summary>
public class SixFeetUnderPower : InsatiablePowerModel
{
	private const int SwallowThreshold = 6;

	private class Data
	{
		public Dictionary<Creature, int> quickSandCounts = new();
	}

	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;

	protected override object InitInternalData() => new Data();

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
	];

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (power is not QuickSandPower) return;
		if (applier is null) return;
		if (amount <= 0) return;

		Creature target = power.Owner;
		Data data = GetInternalData<Data>();

		if (!data.quickSandCounts.ContainsKey(target))
			data.quickSandCounts[target] = 0;
		data.quickSandCounts[target] += 1;
		if (data.quickSandCounts[target] >= SwallowThreshold)
		{
			Flash();
			await CreatureCmd.TriggerAnim(applier, "EatPlayer", 0.5f);
			await Cmd.Wait(2f);
			await TheInsatiableCmd.SwallowCreature(target);
			// 吞噬后重置该敌人的计数，防止重复触发
			data.quickSandCounts[target] = 0;
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner)) return;

		// 回合结束：重置所有敌人的流沙计数
		GetInternalData<Data>().quickSandCounts.Clear();

		// 持续回合数减一，到 0 则移除
		await PowerCmd.Decrement(this);
	}
}
