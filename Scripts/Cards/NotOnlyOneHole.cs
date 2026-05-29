using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class NotOnlyOneHole : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
    public NotOnlyOneHole()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> cards = choesnpile2.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel chosenPileCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (chosenPileCard == null)
        {
            return;
        }
        await ((IChoosable)chosenPileCard).OnChosen(choiceContext);
        if (choiceContext == null)
        {
            return;
        }
        CardPile targetPile;
        if (chosenPileCard is InsatiableDrawPile)
        {
            targetPile = PileType.Discard.GetPile(base.Owner);
        }
        else if (chosenPileCard is InsatiableDiscardPile)
        {
            targetPile = PileType.Draw.GetPile(base.Owner);
        }
        else
        {
            return;
        }
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 2);
        List<CardModel> selectedCards = (List<CardModel>)await CardSelectCmd.FromSimpleGrid(choiceContext, targetPile.Cards, base.Owner, prefs);
        foreach (CardModel card in selectedCards)
        {
            if (card != null)
		    {
                await CardPileCmd.Add(card, PileType.Hand);
            }
        }
    }
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}