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

public class MakingSandcastles : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MakingSandcastlesPower>(2)];

	public MakingSandcastles()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<MakingSandcastlesPower>(choiceContext, base.Owner.Creature, base.DynamicVars["MakingSandcastlesPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars["MakingSandcastlesPower"].UpgradeValueBy(1);
	}
}
