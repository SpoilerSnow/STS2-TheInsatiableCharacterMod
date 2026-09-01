using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class ShrinkerLaser : InsatiableCardModel
{
	public ShrinkerLaser() 
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
		TheInsatiableKeyword.Insect,
		CardKeyword.Exhaust];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(11, ValueProp.Move),
        new DynamicVar("DamageDecrease", 30m),
		new DynamicVar("Count", 4)];
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/shrinker_beetle/shrinker_beetle_cast")
			.WithHitFx("vfx/vfx_attack_slash")
			.Execute(choiceContext);
		await PowerCmd.Apply<ShrinkPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars["Count"].BaseValue, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3);
        base.DynamicVars["Count"].UpgradeValueBy(1);
	}
}
