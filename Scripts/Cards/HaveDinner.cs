using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class HaveDinner : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    public HaveDinner()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            CardModel? swallowedCard = await ((IChoosable)cardModel).OnChosen(choiceContext);
            if (swallowedCard != null && choiceContext != null)
            {
                await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
            }
        }
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.Cards.UpgradeValueBy(1);
	}
}