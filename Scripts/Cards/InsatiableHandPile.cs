using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInsatiable.Scripts;

[Pool(typeof(StatusCardPool))]

public class InsatiableHandPile : InsatiableCardModel, InsatiableCardModel.IChoosable
{
    public override int MaxUpgradeLevel => 0;
	public override bool CanBeGeneratedInCombat => false;
    public InsatiableHandPile()
		: base(-1, CardType.Skill, CardRarity.Token, TargetType.Self, false)
	{
	}
	public virtual async Task<CardModel?> OnChosen(PlayerChoiceContext choiceContext)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		CardPile pile = PileType.Hand.GetPile(base.Owner);
		CardModel cardModel3 = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs)).FirstOrDefault();
        if (cardModel3 != null)
		{
			await TheInsatiableCmd.SwallowCard(choiceContext, cardModel3);
			return cardModel3;
		}
		return null;
    }
}