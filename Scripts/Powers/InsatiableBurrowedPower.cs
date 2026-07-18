using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class InsatiableBurrowedPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Single;

	public override bool ShouldClearBlock(Creature creature)
	{
		if (base.Owner != creature)
		{
			return true;
		}
		return false;
	}
	public override async Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, Creature? breaker)
	{
		if (target == base.Owner)
        {
			await PowerCmd.Remove<InsatiableBurrowedPower>(base.Owner);
		}
	}
}