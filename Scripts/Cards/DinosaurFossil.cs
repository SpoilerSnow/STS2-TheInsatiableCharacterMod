using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class DinosaurFossil : InsatiableCardModel
{
    public override bool CanBeGeneratedInCombat => false;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DinosaurFossilPower>(1)];
	public DinosaurFossil()
		: base(5, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);  
		await PowerCmd.Apply<DinosaurFossilPower>(choiceContext, base.Owner.Creature, base.DynamicVars["DinosaurFossilPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}