using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Piles;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class FullyAbsorb : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		base.EnergyHoverTip
	];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new EnergyVar(1),
		new MaxCapacityVar(5),
	];
	public FullyAbsorb()
		: base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		SwallowPile.MaxCapacity += (int)DynamicVars["MaxCapacity"].BaseValue;
		await PowerCmd.Apply<FullyAbsorbPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}