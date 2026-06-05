using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public class FullyAbsorbPower : InsatiablePowerModel
{
	private class Data
	{
		public int selfSwallowCount;
	}
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	protected override object InitInternalData()
	{
		return new Data();
	}
    public override async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        if (card.Owner.Creature == base.Owner)
		{
			if (causedBySelfSwallow)
			{
				GetInternalData<Data>().selfSwallowCount++;
			}
			else
			{
				await PlayerCmd.GainEnergy(base.Amount, base.Owner.Player);
			}
		}
    }
	public override async Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
	{
		await PlayerCmd.GainEnergy(base.Amount, base.Owner.Player);
    }
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(base.Owner))
		{
			Data data = GetInternalData<Data>();
			await PlayerCmd.GainEnergy(data.selfSwallowCount * base.Amount, base.Owner.Player);
			data.selfSwallowCount = 0;
		}
	}
}