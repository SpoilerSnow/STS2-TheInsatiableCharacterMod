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

public class SandBlowing : InsatiableCardModel
{
    public SandBlowing()
		: base(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)
	{
	}
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => IsUpgraded ? [HoverTipFactory.FromPower<QuickSandPower>(), HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<WeakPower>()] : [HoverTipFactory.FromPower<QuickSandPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<QuickSandPower>(5),
		new PowerVar<VulnerablePower>(1),
		new PowerVar<WeakPower>(1),
	];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
		await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
		if (IsUpgraded)
		{
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
		}
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(2);
	}
}
