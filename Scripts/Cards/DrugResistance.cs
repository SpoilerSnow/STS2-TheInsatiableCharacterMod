using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(Pools.InsatiableCardPool))]

public class DrugResistance : InsatiableCardModel
{
	public DrugResistance()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<DrugResistancePower>(choiceContext, base.Owner.Creature, 1, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		AddKeyword(CardKeyword.Innate);
	}
}