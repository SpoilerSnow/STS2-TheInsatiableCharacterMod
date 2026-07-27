using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class SwarmUp : InsatiableCardModel
{
    public SwarmUp()
		: base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain,
        TheInsatiableKeyword.Insect
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Move),
        new RepeatVar(7),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(base.DynamicVars.Repeat.IntValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .OnlyPlayAnimOnce()
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged")
            .Execute(choiceContext);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Repeat.UpgradeValueBy(1);
	}

}