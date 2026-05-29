using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheInsatiable.Scripts;

public class MuddyPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public decimal ModifyQuickSandDecrease(decimal decrease, Creature? target)
    {
        Creature owner = base.Owner;
        if (target != owner && !owner.Pets.Contains<Creature>(target))
        {
            return decrease;
        }
        decimal multiplier = 1m;
        for (int i = 0; i < Amount; i++)
        {
            multiplier += 1m;
        }
        return decrease * multiplier;
    }
    public decimal ModifyQuickSandIncrease(decimal increase, Creature? dealer)
    {
        Creature owner = base.Owner;
        if (dealer != owner && !owner.Pets.Contains<Creature>(dealer))
        {
            return increase;
        }
        decimal multiplier = 1m;
        for (int i = 0; i < Amount; i++)
        {
            multiplier += 1m;
        }
        return increase * multiplier;
    }
}