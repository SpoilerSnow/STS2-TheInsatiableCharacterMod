using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;
public class QuickSandPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DynamicVar("DamageIncrease", 0.04m),
		new DynamicVar("DamageDecrease", 0.04m),
		new DynamicVar("quicksand1", 0),
		new DynamicVar("quicksand2", 0)
	];
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
		if (base.CombatState == null)
		{
			return;
		}
		base.DynamicVars["quicksand2"].BaseValue = 4 * Amount;
		if (base.CombatState.PlayerCreatures == null)
        {
			return;
		}
		foreach (var player in base.CombatState.PlayerCreatures)
        {
            int muddyamount = player.GetPowerAmount<MuddyPower>();
			int totalmuddyamount = 0;
			if (muddyamount > 0)
			{
				totalmuddyamount += muddyamount;
				base.DynamicVars["quicksand1"].BaseValue = 4 * Amount * (totalmuddyamount + 1);
			}
			else
			{
				base.DynamicVars["quicksand1"].BaseValue = 4 * Amount;
			}
        }
    }
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (dealer == base.Owner)
		{
			decimal numdecrease = 1m;
			decimal decrease = base.DynamicVars["DamageDecrease"].BaseValue;
			MuddyPower muddy = target?.GetPower<MuddyPower>();
			if (muddy != null)
			{
				decrease = muddy.ModifyQuickSandDecrease(decrease, target);
			}
			for (int i = 0; i < Amount; i++)
			{
				numdecrease -= decrease;
			}
			return Math.Max(0m, numdecrease);
		}
		if (target == base.Owner && dealer?.IsPlayer == true && dealer.HasPower<ScorchedEarthPower>())
		{
			decimal numincrease = 1m;
			decimal increase = base.DynamicVars["DamageIncrease"].BaseValue;
			MuddyPower muddy = dealer?.GetPower<MuddyPower>();
			if (muddy != null)
			{
				increase = muddy.ModifyQuickSandIncrease(increase, dealer);
			}
			ScorchedEarthPower scorchedEarth = dealer?.GetPower<ScorchedEarthPower>();
			for (int i = 0; i < Amount; i++)
			{
				numincrease += increase*scorchedEarth.Amount;
			}
			return numincrease;
		}
		return 1m;
	} 
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
		if (side == base.Owner.Side)
		{
			int oldAmount = Amount;
            int newAmount = oldAmount / 2;
		    await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), this, newAmount - Amount, null, null);
		}
    }
}

