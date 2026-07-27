using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class QuickThorns : InsatiableCardModel
{
	public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(7, ValueProp.Move),
        new PowerVar<ThornsPower>(2),
        new PowerVar<QuickThornsPower>(2)
    ];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<ThornsPower>()
    ];
	public QuickThorns() 
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<ThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<QuickThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["QuickThornsPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(1);
		DynamicVars["QuickThornsPower"].UpgradeValueBy(3);
	}
}
