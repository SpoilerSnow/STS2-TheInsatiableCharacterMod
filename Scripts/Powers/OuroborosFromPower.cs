using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheInsatiable.Scripts;

public class OuroborosFromPower : InsatiablePowerModel
{
    private class Data
	{
		public int selfSwallowCount;
	}
    private void UpgradeAllCardDynamicVars(int amount)
    {
        foreach (CardModel allCard in base.Owner.Player.PlayerCombatState.AllCards)
        {
		    foreach (DynamicVar value in allCard.DynamicVars.Values)
            {
                value.UpgradeValueBy(amount);
            }
            if (allCard.BaseReplayCount > 0)
		    {
	            allCard.BaseReplayCount = allCard.BaseReplayCount + amount;
            }
            NCard val = NCard.FindOnTable(allCard, null);
            if (val == null)
            {
                continue;
            }
            val.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
        }
    }
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
                UpgradeAllCardDynamicVars(base.Amount);
			}
		}
    }
	public override async Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
	{
        UpgradeAllCardDynamicVars(base.Amount);
    }
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(base.Owner))
		{
			Data data = GetInternalData<Data>();
            int amount = data.selfSwallowCount * base.Amount;
            if (amount > 0)
            {
                UpgradeAllCardDynamicVars(amount);
            }
			data.selfSwallowCount = 0;
		}
	}   
}