using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public sealed class MawOfVoidPower : InsatiablePowerModel
{
    private PlayerChoiceContext choiceContext;

    public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
	{
		modifiedCost = originalCost;
		if (card.Owner.Creature != base.Owner)
		{
			return false;
		}
		bool flag;
		switch (card.Pile?.Type)
		{
		case PileType.Hand:
		case PileType.Play:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			return false;
		}
		modifiedCost = default(decimal);
		return true;
	}

	public override async Task BeforeCardPlayed(CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner.Creature == base.Owner)
		{
			bool flag;
			switch (cardPlay.Card.Pile?.Type)
			{
			case PileType.Hand:
			case PileType.Play:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
		}
	}
	public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await TheInsatiableCmd.SwallowCard(choiceContext, cardPlay.Card);
		await PowerCmd.Decrement(this);
	}
}