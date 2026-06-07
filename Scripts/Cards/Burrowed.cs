using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class Burrowed : InsatiableCardModel
{
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<QuickSandPower>()
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(32, ValueProp.Move),
        new PowerVar<QuickSandPower>(4),
        new PowerVar<InsatiableBurrowedPower>(1)
    ];
    public Burrowed()
		: base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<InsatiableBurrowedPower>(choiceContext, base.Owner.Creature, base.DynamicVars["InsatiableBurrowedPower"].BaseValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(5);
	}
}