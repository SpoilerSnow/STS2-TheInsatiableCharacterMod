using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class Slumber : InsatiableCardModel
{
    public override bool GainsBlock => true;
    public Slumber() 
		: base(2, CardType.Skill, CardRarity.Uncommon,  TargetType.Self, true)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Innate,
        TheInsatiableKeyword.SelfSwallow,
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(15, ValueProp.Move),
        new PowerVar<StrengthPower>(2),
    ];
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.IntValue, base.Owner.Creature, this);
        PlayerCmd.EndTurn(base.Owner, canBackOut: false);
	}
	protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(3);
		RemoveKeyword(TheInsatiableKeyword.SelfSwallow);
	}
}