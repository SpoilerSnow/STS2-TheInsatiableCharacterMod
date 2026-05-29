using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheInsatiable.Scripts;
public class MobileDunePower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
        if (participants.Contains(base.Owner))
		{
		    if (side != base.Owner.Side)
		    {
			    return;
		    }
            var enemies = combatState.Enemies.Where(c => c.IsAlive).ToList();
            if (enemies.Count == 0)
            {
                return;
            }
            int totalSand = enemies.Sum(c => c.GetPowerAmount<QuickSandPower>());
            int average = totalSand / enemies.Count;
            foreach (var enemy in enemies)
            {
                int currentSand = enemy.GetPowerAmount<QuickSandPower>();
                int delta = average - currentSand;
                if (delta == 0)
                {
                    continue;
                }
                if (currentSand == 0)
                {
                    await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), enemy, average, base.Owner, null);
                    continue;
                }
                var quickSandPower = enemy.GetPower<QuickSandPower>();
                if (quickSandPower == null)
                {
                    continue;
                }
                if (delta != 0)
                {
                    await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), quickSandPower, delta, base.Owner, null);
                }
            }
        }
    }
}
