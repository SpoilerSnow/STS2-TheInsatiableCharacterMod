using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;
public class SandFlowsPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner))
		{
			return;
		}
		foreach (var enemy in base.CombatState.HittableEnemies)
        {
			if (enemy.IsAlive && enemy.GetPower<QuickSandPower>() != null)
			{
			int quicksandCount = enemy.GetPower<QuickSandPower>().Amount;
            int damage = quicksandCount * base.Amount;
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), enemy, damage, ValueProp.Unpowered, null, null);
			if (!base.Owner.IsAlive)
			{
				await Cmd.CustomScaledWait(0.1f, 0.25f);
			}
			}
		}
	}
}