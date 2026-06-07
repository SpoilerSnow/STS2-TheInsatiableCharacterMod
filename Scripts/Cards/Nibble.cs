using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(StatusCardPool))]

public class Nibble : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(4),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.PlayerCombatState?.ExhaustPile.Cards.Count((CardModel c) => c is Hatch) ?? 0)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Hatch>()];
    public Nibble()
		: base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.CalculationBase.UpgradeValueBy(1);
	}
}