using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class SandFlowsPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner))
		{
			return;
		}
		foreach (var enemy in base.CombatState.HittableEnemies)
        {
			int damage = enemy.GetPower<QuickSandPower>()?.CalculateTotalDamageNextTurn() ?? 0;
			if (enemy.IsAlive && enemy.GetPower<QuickSandPower>() != null)
			{
			    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), enemy, damage, ValueProp.Unpowered, null, null);
			    if (!base.Owner.IsAlive)
			    {
				    await Cmd.CustomScaledWait(0.1f, 0.25f);
			    }
			}
		}
	}
}