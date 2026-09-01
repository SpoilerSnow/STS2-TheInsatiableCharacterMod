using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Burrowed : InsatiableCardModel
{
    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        TheInsatiableKeyword.Insect,
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(27, ValueProp.Move),
    ];
    public Burrowed()
		: base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_burrow");
        await PowerCmd.Apply<InsatiableBurrowedPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(5);
	}
}