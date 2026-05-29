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

public class DesertMirage : InsatiableCardModel
{
    public override bool CanBeGeneratedInCombat => false;
    public override bool GainsBlock => true;
    public DesertMirage()
        : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
    {
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(4),
        new BlockVar(10, ValueProp.Move),
        new PowerVar<QuickSandPower>(6),
        new PowerVar<DesertMiragePower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<DesertMiragePower>(choiceContext, base.Owner.Creature, base.DynamicVars["DesertMiragePower"].IntValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Heal.UpgradeValueBy(2);
        base.DynamicVars.Block.UpgradeValueBy(2);
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(2);
    }
}