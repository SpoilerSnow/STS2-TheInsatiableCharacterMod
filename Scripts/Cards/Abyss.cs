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

public class Abyss : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<LocustOfWar>(base.IsUpgraded),
        HoverTipFactory.FromCard<LocustOfConquest>(base.IsUpgraded),
        HoverTipFactory.FromCard<LocustOfFamine>(base.IsUpgraded),
        HoverTipFactory.FromCard<LocustOfPestilence>(base.IsUpgraded),
        HoverTipFactory.FromCard<LocustOfDeath>(base.IsUpgraded),
        HoverTipFactory.FromCard<LocustOfAbyss>(base.IsUpgraded),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public Abyss()
		: base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        for (int i = 0; i < 6; i++)
		{
            List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
            CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
            if (cardModel == null)
            {
                continue;
            }
            CardModel? exhaustedCard = await ((IChoosable)cardModel).OnChosen(choiceContext);
            if (exhaustedCard != null && !(exhaustedCard is Abyss))
            {
                await AddLocustByExhaustedCardType(choiceContext, exhaustedCard.Type);
            }
            if (exhaustedCard != null && exhaustedCard is Abyss)
            {
                List<CardModel> abyss = new()
                {
                    base.CombatState.CreateCard<LocustOfWar>(base.Owner),
                    base.CombatState.CreateCard<LocustOfConquest>(base.Owner),
                    base.CombatState.CreateCard<LocustOfFamine>(base.Owner),
                    base.CombatState.CreateCard<LocustOfPestilence>(base.Owner),
                    base.CombatState.CreateCard<LocustOfDeath>(base.Owner),
                    base.CombatState.CreateCard<LocustOfAbyss>(base.Owner),
                };
                if (base.IsUpgraded)
                {
                    foreach (var loc in abyss)
                    {
                        CardCmd.Upgrade(loc);
                    }
                }
                foreach (var loc in abyss)
                {
                    await CardPileCmd.AddGeneratedCardToCombat(loc, PileType.Hand, base.Owner);
                }
            }
        }
    }
    private async Task AddLocustByExhaustedCardType(PlayerChoiceContext choiceContext, CardType exhaustedType)
    {
        CardModel locust = exhaustedType switch
        {
            CardType.Attack => base.CombatState.CreateCard<LocustOfWar>(base.Owner),
            CardType.Skill => base.CombatState.CreateCard<LocustOfFamine>(base.Owner),
            CardType.Power => base.CombatState.CreateCard<LocustOfConquest>(base.Owner),
            CardType.Status => base.CombatState.CreateCard<LocustOfPestilence>(base.Owner),
            CardType.Curse => base.CombatState.CreateCard<LocustOfDeath>(base.Owner),
            CardType.Quest => base.CombatState.CreateCard<LocustOfAbyss>(base.Owner),
        };
        if (base.IsUpgraded)
		{
			CardCmd.Upgrade(locust);
		}
        await CardPileCmd.AddGeneratedCardToCombat(locust, PileType.Hand, base.Owner);
    }
}