using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class SixFeetUnder : InsatiableCardModel
{
	private const int energyCost = 2;
	private const CardType type = CardType.Skill;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;

	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain,CardKeyword.Exhaust];

	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<SixFeetUnderPower>(1),
	];

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
	];

	public SixFeetUnder()
		: base(energyCost, type, rarity, targetType)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<SixFeetUnderPower>(choiceContext, cardPlay.Target, 1, base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
