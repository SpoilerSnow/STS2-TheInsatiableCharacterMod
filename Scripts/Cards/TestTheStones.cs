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

public class TestTheStones : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Exhaust,
		TheInsatiableKeyword.Insect
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<ImbalancedPower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];
	public TestTheStones() 
		: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_attack")
			.WithHitFx("vfx/vfx_rock_shatter", null, "blunt_attack.mp3")
			.Execute(choiceContext);
        await PowerCmd.Apply<ImbalancedPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, 1, base.Owner.Creature, this);
		await PowerCmd.Apply<TestTheStonesPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, 1, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4);
	}
}