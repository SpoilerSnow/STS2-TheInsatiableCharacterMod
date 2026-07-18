using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Piles;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Appetizer : InsatiableCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Innate,
		CardKeyword.Exhaust,
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)
    ];
	public Appetizer()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int innateCount = PileType.Hand.GetPile(base.Owner).Cards.Count(card => card.Keywords.Contains(CardKeyword.Innate)) + 1;
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, innateCount);
        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		List<CardModel> cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner, prefs)).ToList();
        if (cardModel != null)
		{
			foreach (CardModel card in cardModel)
            {
			    bool swallowed = await TheInsatiableCmd.SwallowCard(choiceContext, card);
			}
		}
		if (IsUpgraded)
		{
			await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), base.DynamicVars.Cards.BaseValue, base.Owner);
		}
	}
}