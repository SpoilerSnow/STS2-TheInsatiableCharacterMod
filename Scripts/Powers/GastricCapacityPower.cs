using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class GastricCapacityPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => true;
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (applier == base.Owner && power is GastricCapacityPower)
		{
			Flash();
            if (!SwallowPile.IsLocked)
            {
                SwallowPile.MaxCapacity += (int)amount;
            }
		}
	}
}