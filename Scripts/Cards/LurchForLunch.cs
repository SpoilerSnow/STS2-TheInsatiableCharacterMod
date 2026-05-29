using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class LurchForLunch : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new CardsVar(1),
		new EnergyVar(1)
	];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<InsatiableSwallow>(),
        base.EnergyHoverTip
    ];
    public LurchForLunch()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<InsatiableSwallow>(base.Owner), PileType.Hand, base.Owner);
    }
    protected override void OnUpgrade()
	{
		AddKeyword(CardKeyword.Retain);
    }
}