using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]
[RegisterArchaicToothTranscendence(typeof(SandBlowing))]

public class SandStorm : InsatiableCardModel
{
    public SandStorm()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.AllEnemies)
	{
	}
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>(),
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
		await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(3);
        base.DynamicVars.Weak.UpgradeValueBy(1);
	    base.DynamicVars.Vulnerable.UpgradeValueBy(1);
	}
}
