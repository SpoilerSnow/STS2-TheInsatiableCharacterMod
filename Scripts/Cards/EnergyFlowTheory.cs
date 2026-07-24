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

public class EnergyFlowTheory : InsatiableCardModel
{
	public override bool CanBeGeneratedInCombat => false;
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EnergyFlowTheoryPower>(6)];
	public EnergyFlowTheory()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<EnergyFlowTheoryPower>(choiceContext, base.Owner.Creature, base.DynamicVars["EnergyFlowTheoryPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.DynamicVars["EnergyFlowTheoryPower"].UpgradeValueBy(1);
    }
}