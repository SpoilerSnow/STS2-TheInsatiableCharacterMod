using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheInsatiable.Scripts;

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
	public override async Task AfterBlockBroken(Creature creature)
	{
		if (creature == base.Owner)
		{
			await PowerCmd.Remove<InsatiableBurrowedPower>(base.Owner);
		}
	}
}