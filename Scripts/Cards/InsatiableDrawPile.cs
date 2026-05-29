using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInsatiable.Scripts;

[Pool(typeof(StatusCardPool))]

public class InsatiableDrawPile : InsatiableCardModel, InsatiableCardModel.IChoosable
{
    public override int MaxUpgradeLevel => 0;
	public override bool CanBeGeneratedInCombat => false;
    public InsatiableDrawPile()
		: base(-1, CardType.Skill, CardRarity.Token, TargetType.Self, false)
	{
	}
	public virtual async Task<CardModel?> OnChosen(PlayerChoiceContext choiceContext)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		CardModel cardModel2 = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner, prefs)).FirstOrDefault();
        if (cardModel2 != null)
		{
			await TheInsatiableCmd.SwallowCard(choiceContext, cardModel2);
			return cardModel2;
		}
		return null;
    }
}