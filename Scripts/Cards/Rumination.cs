using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts;

[RegisterCard(typeof(InsatiableCardPool))]

public class Rumination : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.SelfSwallow)
    ];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<RuminationPower>(1),
		new CardsVar(5)
	];
	public Rumination()
		: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		SwallowPile.MaxCapacity += (int)base.DynamicVars.Cards.BaseValue;
		await PowerCmd.Apply<RuminationPower>(choiceContext, base.Owner.Creature, base.DynamicVars["RuminationPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
