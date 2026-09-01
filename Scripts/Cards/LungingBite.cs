using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class LungingBite : InsatiableCardModel
{
    protected override bool ShouldGlowGoldInternal => base.Owner.Creature.CurrentHp <= base.Owner.Creature.MaxHp * 0.5;
	public LungingBite() 
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(28, ValueProp.Move)];
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
	{
		if (base.Owner.Creature.CurrentHp <= base.Owner.Creature.MaxHp * 0.5 && cardSource == this)
        {
            return 2m;
        }
		return 1m;
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(6);
	}
}
