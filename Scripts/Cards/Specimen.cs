using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Specimen : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => 
	[
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Insect),
		HoverTipFactory.FromKeyword(CardKeyword.Retain),
        ..HoverTipFactory.FromEnchantment<PerfectFit>()
    ];
    public Specimen() 
		: base(1, CardType.Skill, CardRarity.Uncommon,  TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IEnumerable<CardModel> cardModel = CardFactory.GetDistinctForCombat(base.Owner, from c in ModelDb.CardPool<InsatiableCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
			where c.Keywords.Contains(TheInsatiableKeyword.Insect)
			select c, base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration);
		foreach (CardModel item in cardModel)
		{
			CardCmd.ApplyKeyword(item, CardKeyword.Retain);
            CardCmd.Enchant<PerfectFit>(item, 1m);
			await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, base.Owner);
		}
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars.Cards.UpgradeValueBy(1);
	}
}