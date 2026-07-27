using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class CrystalClear : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<CrystalClearPower>(2),
		new PowerVar<QuickSandPower>(5),
	];
	public CrystalClear()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<CrystalClearPower>(choiceContext, base.Owner.Creature, base.DynamicVars["CrystalClearPower"].BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<QuickSandPower>(choiceContext, cardPlay.Target, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars["CrystalClearPower"].UpgradeValueBy(1);
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(1);
	}
}