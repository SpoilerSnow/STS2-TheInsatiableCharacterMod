using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class TestTheStones : InsatiableCardModel
{
	private const int energyCost = 2;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ImbalancedPower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];
	public TestTheStones() 
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/workbug_rock/workbug_rock_attack")
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
        await PowerCmd.Apply<ImbalancedPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, 1, base.Owner.Creature, this);
		await PowerCmd.Apply<TestTheStonesPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, 1, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4);
	}
}