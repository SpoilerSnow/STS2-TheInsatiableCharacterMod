using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class EggAgainstStone : InsatiableCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.Insect];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(7, ValueProp.Move),
		new PowerVar<PlatingPower>(4),
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];
	public EggAgainstStone()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/workbug_egg/workbug_egg_attack")
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
		await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<EggAgainstStonePower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2);
		base.DynamicVars["PlatingPower"].UpgradeValueBy(1);
	}
}
