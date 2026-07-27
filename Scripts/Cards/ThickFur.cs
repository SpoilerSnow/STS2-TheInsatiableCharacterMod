using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class ThickFur : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.Insect];
	public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new BlockVar(8, ValueProp.Move),
		new BlockVar("BlockNextTurn", 7, ValueProp.Move),
        new PowerVar<BlurPower>(1),
        new PowerVar<PlatingPower>(2),
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Gulp),
        HoverTipFactory.FromPower<BlurPower>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest),
        HoverTipFactory.FromPower<PlatingPower>()
    ];
	public ThickFur() 
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        BlockVar blockVar = (BlockVar)base.DynamicVars["BlockNextTurn"];
        IEnumerable<AbstractModel> modifiers;
        decimal blockNextTurnAmount = Hook.ModifyBlock(base.CombatState, base.Owner.Creature, blockVar.BaseValue, blockVar.Props, this, cardPlay, out modifiers);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, base.Owner.Creature, blockNextTurnAmount, base.Owner.Creature, this);
	}
    public override async Task OnGulp()
    {
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		await PowerCmd.Apply<BlurPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["BlurPower"].BaseValue, base.Owner.Creature, this);
	}
    public override async Task OnDigest()
    {
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars["BlockNextTurn"].UpgradeValueBy(2);
	}
}