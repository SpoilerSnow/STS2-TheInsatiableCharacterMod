using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInsatiable.Scripts;

[Pool(typeof(StatusCardPool))]

public class InsatiableDiscardPile : InsatiableCardModel, InsatiableCardModel.IChoosable
{
    public override int MaxUpgradeLevel => 0;
	public override bool CanBeGeneratedInCombat => false;
    public InsatiableDiscardPile()
		: base(-1, CardType.Skill, CardRarity.Token, TargetType.Self, false)
	{
	}
	public virtual async Task<CardModel?> OnChosen(PlayerChoiceContext choiceContext)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		CardPile pile = PileType.Discard.GetPile(base.Owner);
		CardModel cardModel1 = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs)).FirstOrDefault();
        if (cardModel1 != null)
		{
			await TheInsatiableCmd.SwallowCard(choiceContext, cardModel1);
			return cardModel1;
		}
		return null;
    }
}