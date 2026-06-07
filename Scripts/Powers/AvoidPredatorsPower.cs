using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

public class AvoidPredatorsPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        Creature owner = base.Owner;
        if (target != owner || ((canonicalPower is not FrailPower) && (canonicalPower is not WeakPower) && (canonicalPower is not VulnerablePower)) || !canonicalPower.IsVisible)
        {
            modifiedAmount = amount;
            return false;
        }      
        modifiedAmount = 0m;
        Flash();
        return true;
    }
    public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
	{
		await PowerCmd.Decrement(this);
	}
}