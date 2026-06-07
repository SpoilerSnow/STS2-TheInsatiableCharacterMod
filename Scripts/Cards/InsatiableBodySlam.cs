using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class InsatiableBodySlam : InsatiableCardModel
{
	private const int energyCost = 1;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Rare;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;
    public InsatiableBodySlam()
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<QuickSandPower>(4),
		new CalculationBaseVar(0),
		new ExtraDamageVar(1),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => card.Owner.Creature.Block)
	];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromPower<QuickSandPower>()
		];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        var targets = base.CombatState.HittableEnemies
        .Where(enemy => enemy.HasPower<QuickSandPower>())
        .ToList();
        foreach (var enemy in targets)
        {
            await DamageCmd
            .Attack(base.DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(enemy)
			.WithHitFx("vfx/vfx_bite")
            .Execute(choiceContext);
        }
    }
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}