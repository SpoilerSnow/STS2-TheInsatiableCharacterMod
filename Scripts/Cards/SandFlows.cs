using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class SandFlows : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromPower<SandySkyPower>()
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<SandFlowsPower>(1),
        new PowerVar<SandySkyPower>(3)
    ];
	public SandFlows()
		: base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<SandFlowsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SandFlowsPower"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<SandySkyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SandySkyPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.DynamicVars["SandySkyPower"].UpgradeValueBy(1);
    }
}
