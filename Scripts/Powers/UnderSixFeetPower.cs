using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class UnderSixFeetPower : InsatiablePowerModel
{
	private const int SwallowThreshold = 6;

	private class Data
	{
		public Dictionary<Creature, int> quickSandCounts = new();
	}
	public override int DisplayAmount => GetInternalData<Data>().quickSandCounts.GetValueOrDefault(base.Owner, 0);
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Counter;

	protected override object InitInternalData() => new Data();

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
	];

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		Creature target = base.Owner;
		if (power.Owner != target) return;
		if (power is not QuickSandPower) return;
		if (applier is null) return;
		if (amount <= 0) return;
		Data data = GetInternalData<Data>();
		if (!data.quickSandCounts.ContainsKey(target))
			data.quickSandCounts[target] = 0;
		data.quickSandCounts[target] += 1;
		InvokeDisplayAmountChanged();
		if (data.quickSandCounts[target] >= SwallowThreshold)
		{
			Flash();
			await CreatureCmd.TriggerAnim(applier, "EatPlayer", 0.5f);
			await Cmd.Wait(2f);
			await TheInsatiableCmd.SwallowCreature(target);
			data.quickSandCounts[target] = 0;
			InvokeDisplayAmountChanged();
		}
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner)) return;
		GetInternalData<Data>().quickSandCounts.Clear();
		InvokeDisplayAmountChanged();
	}
}
