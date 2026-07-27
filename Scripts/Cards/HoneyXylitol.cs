using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class HoneyXylitol : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.Insect];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Gulp),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<HoneyXylitolPower>(2),
        new PowerVar<StrengthPower>(2),
    ];
    public HoneyXylitol()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<HoneyXylitolPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["HoneyXylitolPower"].IntValue, base.Owner.Creature, this);
	}
	public override async Task OnGulp()
	{
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.IntValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars["HoneyXylitolPower"].UpgradeValueBy(1);
        base.DynamicVars.Strength.UpgradeValueBy(1);
	}
}