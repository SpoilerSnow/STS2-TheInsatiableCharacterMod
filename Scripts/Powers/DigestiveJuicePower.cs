using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
public class DigestiveJuicePower : InsatiablePowerModel
{
	int num = 0;
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
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
		num++;
		if (num <= 1)
		{
			Flash();
            await PowerCmd.TickDownDuration(this);
		}
        if (base.Amount <= 0)
        {
            Flash();
			await TheInsatiableCmd.SwallowCreature(applier, target);
        }
	}
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner)) return;
		num = 0;
	}
}