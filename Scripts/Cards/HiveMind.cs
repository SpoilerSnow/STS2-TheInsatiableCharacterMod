using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class HiveMind : InsatiableCardModel
{
	public HiveMind()
		: base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<HiveMindPower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}