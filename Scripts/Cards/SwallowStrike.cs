using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;


namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class SwallowStrike : InsatiableCardModel
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new CalculationBaseVar(7),
		new ExtraDamageVar(4),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? target)
		{
			var history = CombatManager.Instance.History;
            var swallowedCards = history.Entries.OfType<CardSwallowedEntry>().Count(e => e.Card.Owner == card.Owner);
            var swallowedCreatures = history.Entries.OfType<CreatureSwallowedEntry>().Count();
			int num = swallowedCards + swallowedCreatures;
            return num;
		})
    ];
	public SwallowStrike() 
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd
            .Attack(base.DynamicVars.CalculatedDamage)
			.FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.ExtraDamage.UpgradeValueBy(1);
    }
}