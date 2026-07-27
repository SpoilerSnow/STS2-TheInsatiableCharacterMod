using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Powers;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(TokenCardPool))]

public class NegativePhototaxis : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];
    public NegativePhototaxis()
		: base(0, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
		await PowerCmd.Apply<NegativePhototaxisPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars.Strength.IntValue, base.Owner.Creature, this);
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars.Strength.UpgradeValueBy(1);
	}
}