using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts;

/// <summary>
/// 以卵击石附属监听 Power（Single，不叠加）。
/// 仅负责在拥有者受到未被格挡的攻击伤害时，移除所有覆甲（PlatingPower）。
/// </summary>
[RegisterPower]
public class EggAgainstStonePower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Debuff;

	public override PowerStackType StackType => PowerStackType.Single;

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<PlatingPower>()];

	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target != base.Owner) return;
		if (!props.IsPoweredAttack()) return;
		if (result.UnblockedDamage <= 0) return;

		PlatingPower? plating = base.Owner.GetPower<PlatingPower>();
		if (plating != null)
		{
			Flash();
			await PowerCmd.Remove(plating);
		}
		await PowerCmd.Remove(this);
	}
}
