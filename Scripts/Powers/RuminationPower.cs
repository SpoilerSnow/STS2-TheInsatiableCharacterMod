using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public sealed class RuminationPower : InsatiablePowerModel
{
	private class Data
	{
		public int cardSwallowedThisTurn;
	}
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	protected override object InitInternalData()
	{
		return new Data();
	}
	public override Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		GetInternalData<Data>().cardSwallowedThisTurn = CombatManager.Instance.History.Entries.OfType<CardSwallowedEntry>().Count((CardSwallowedEntry e) => e.Card.Owner.Creature == base.Owner && e.HappenedThisTurn(base.CombatState));
		return Task.CompletedTask;
	}
	public override async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
	{
		if (card.Owner != base.Owner.Player)
		{
			return;
		}
		GetInternalData<Data>().cardSwallowedThisTurn++;
		if (GetInternalData<Data>().cardSwallowedThisTurn == 1)
		{
			Flash();
			for (int i = 0; i < base.Amount; i++)
			{
				CardModel swallowedcard = card.CreateClone();
                CardCmd.ApplyKeyword(swallowedcard, TheInsatiableKeyword.SelfSwallow);
				await CardPileCmd.AddGeneratedCardToCombat(swallowedcard, PileType.Hand, base.Owner.Player);
                
			}
		}
	}
	public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner))
		{
			return Task.CompletedTask;
		}
		GetInternalData<Data>().cardSwallowedThisTurn = 0;
		return Task.CompletedTask;
	}
}