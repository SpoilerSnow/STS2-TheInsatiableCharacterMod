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
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class SwordOfSand : InsatiableCardModel
{
    public SwordOfSand() 
		: base(1, CardType.Attack, CardRarity.Uncommon,  TargetType.AnyEnemy, true)
	{
	}
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? target) =>
            CombatManager.Instance.History.Entries
                .OfType<PowerReceivedEntry>()
                .Where((PowerReceivedEntry e) =>
                    e.HappenedThisTurn(card.CombatState)
                    && e.Power is QuickSandPower
                    && e.Applier == card.Owner.Creature)
                .Sum((PowerReceivedEntry e) => e.Amount))
    ];
	
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd
        .Attack(base.DynamicVars.CalculatedDamage)
        .FromCard(this)
        .Targeting(cardPlay.Target)
		.Execute(choiceContext);
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}