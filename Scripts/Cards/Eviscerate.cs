using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Eviscerate : InsatiableCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => 
	[
		new DamageVar(3, ValueProp.Move),
		new RepeatVar(2)
	];
	public Eviscerate() 
		: base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.WithHitCount(base.DynamicVars.Repeat.IntValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.OnlyPlayAnimOnce()
			.WithAttackerAnim("Thrash", 0.3f)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash")
			.WithHitFx("vfx/vfx_scratch")
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Repeat.UpgradeValueBy(1);
	}
}
