using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using STS2RitsuLib.Combat.CardTargeting;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class InsatiableBodySlam : InsatiableCardModel
{
public InsatiableBodySlam()
		: base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<QuickSandPower>(4),
		new CalculationBaseVar(0),
		new ExtraDamageVar(1),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.Creature.Block)
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromPower<QuickSandPower>()
		];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
			.TargetingFiltered(base.CombatState.HittableEnemies.Where(enemy => enemy.HasPower<QuickSandPower>()).ToList())
			.WithHitFx("vfx/vfx_bite")
            .Execute(choiceContext);
    }
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}