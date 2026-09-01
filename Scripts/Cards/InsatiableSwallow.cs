using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(StatusCardPool))]

public class InsatiableSwallow : InsatiableCardModel
{
    public override int MaxUpgradeLevel => 0;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Piles),
    ];
    public InsatiableSwallow()
		: base(1, CardType.Status, CardRarity.Status, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            await ((IChoosable)cardModel).OnChosen(choiceContext);
        }
    }
}
