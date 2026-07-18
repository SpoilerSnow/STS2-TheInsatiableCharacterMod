using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Piles;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Rumination : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.SelfSwallow)
    ];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<RuminationPower>(1),
		new MaxCapacityVar(4),
	];
	public Rumination()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		SwallowPile.MaxCapacity += (int)DynamicVars["MaxCapacity"].BaseValue;
		await PowerCmd.Apply<RuminationPower>(choiceContext, base.Owner.Creature, base.DynamicVars["RuminationPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        DynamicVars["MaxCapacity"].UpgradeValueBy(4);
    }
}
