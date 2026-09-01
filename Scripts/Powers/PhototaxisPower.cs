using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class PhototaxisPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (power.Type == PowerType.Buff && amount > 0m && applier != null && applier.IsMonster)
		{
			Flash();
			await CardPileCmd.Draw(choiceContext, amount, base.Owner.Player);
            await PowerCmd.Decrement(this);
		}
	}
}