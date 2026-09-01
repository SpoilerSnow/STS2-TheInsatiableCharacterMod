using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class LivingWeapon : InsatiableCardModel
{
    public LivingWeapon()
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, true)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.SelfSwallow];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .OnlyPlayAnimOnce()
			.WithAttackerAnim("Thrash", 0.3f)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash")
			.WithHitFx("vfx/vfx_scratch")
			.Execute(choiceContext);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(3);
	}
}
