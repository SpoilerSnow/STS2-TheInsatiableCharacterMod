using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class SandStorm : InsatiableCardModel
{
	private const int energyCost = 1;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Ancient;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;

    public SandStorm()
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
        ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<QuickSandPower>(7),
        new PowerVar<WeakPower>(1),
        new PowerVar<VulnerablePower>(1)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(3);
        base.DynamicVars.Weak.UpgradeValueBy(1);
	    base.DynamicVars.Vulnerable.UpgradeValueBy(1);
	}

}