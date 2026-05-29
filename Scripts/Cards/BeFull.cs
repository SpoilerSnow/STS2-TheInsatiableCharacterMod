using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class BeFull : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        base.EnergyHoverTip
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(3),
        new EnergyVar(2)
    ];
    public BeFull()
		: base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            await ((IChoosable)cardModel).OnChosen(choiceContext);
            if (choiceContext == null)
            {
                return;
            }
            else
            {
                await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
                await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            }
        }
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.Cards.UpgradeValueBy(1);
        base.DynamicVars.Energy.UpgradeValueBy(1);
	}
}