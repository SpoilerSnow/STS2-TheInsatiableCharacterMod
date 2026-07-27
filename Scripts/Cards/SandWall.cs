using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class SandWall : InsatiableCardModel
{
    public override bool GainsBlock => true;
    public SandWall()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromCard<SandStone>()
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(11, ValueProp.Move),
        new PowerVar<QuickSandPower>(4)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<SandStone>(base.Owner), PileType.Hand, base.Owner);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(4);
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(2);
	}

}
