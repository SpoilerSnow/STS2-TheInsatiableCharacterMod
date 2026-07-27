using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Pheromone : InsatiableCardModel
{
    public Pheromone() 
		: base(1, CardType.Skill, CardRarity.Uncommon,  TargetType.Self)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust,
        TheInsatiableKeyword.Insect
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		CardModel cardModel = CardFactory.GetDistinctForCombat(base.Owner, from c in ModelDb.CardPool<InsatiableCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
			where c.Keywords.Contains(TheInsatiableKeyword.Insect) && c.Id != this.Id
			select c, 1, base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (IsUpgraded)
        {
            CardCmd.Upgrade(cardModel);
        }
		if (cardModel != null)
		{
			cardModel.SetToFreeThisTurn();
			await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner);
		}
	}
}