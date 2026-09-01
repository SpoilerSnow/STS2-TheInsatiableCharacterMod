using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class StealTechnique : InsatiableCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Exhaust,
		TheInsatiableKeyword.Insect
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(11, ValueProp.Move),
		new CalculationBaseVar(3),
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
	public StealTechnique()
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_steal")
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		await PowerCmd.Apply<StealTechniquePower>(choiceContext, base.Owner.Creature, base.DynamicVars.CalculationBase.BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(4);
	}
}
