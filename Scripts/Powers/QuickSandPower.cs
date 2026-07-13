using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.HealthBars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class QuickSandPower : InsatiablePowerModel, IHealthBarForecastSource
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DynamicVar("DamageIncrease", 0.04m),
		new DynamicVar("DamageDecrease", 0.03m),
		new DynamicVar("quicksand1", 0),
		new DynamicVar("quicksand2", 0)
	];
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
		if (base.CombatState == null)
		{
			return;
		}
		base.DynamicVars["quicksand2"].BaseValue = 3 * Amount;
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
				base.DynamicVars["quicksand1"].BaseValue = 3 * Amount * (totalmuddyamount + 1);
			}
			else
			{
				base.DynamicVars["quicksand1"].BaseValue = 3 * Amount;
			}
        }
    }
	public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
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
	private int SandFlowsPowerCount
	{
		get
		{
			IEnumerable<Creature> source = from c in base.Owner.CombatState.GetOpponentsOf(base.Owner)
				where c.IsAlive
				select c;
			return Math.Max(0, source.Sum((Creature a) => a.GetPowerAmount<SandFlowsPower>()));
		}
	}
	public int CalculateTotalDamageNextTurn()
    {
        decimal num = default;
        decimal damage = base.Amount * SandFlowsPowerCount;
		damage = Hook.ModifyDamage(base.Owner.CombatState.RunState, base.Owner.CombatState, base.Owner, null, damage, ValueProp.Unpowered, null, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
		num += damage;
        return (int)num;
    }
	public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
		if (base.Owner.IsPlayer)
        {
            return Enumerable.Empty<HealthBarForecastSegment>();
        }
        return HealthBarForecasts.Single(
            (context.Creature.GetPower<QuickSandPower>()?.CalculateTotalDamageNextTurn() ?? 0) - context.Creature.Block, // 展示的数量（例如如果你的能力有2倍效果可以乘2）
            new Color(188f / 255f, 130f / 255f, 54f / 255f), // 颜色
            HealthBarForecastGrowthDirection.FromRight // 从左边开始延伸还是右边开始
        // 0, // 顺序，越大越远离血条边缘，默认0
        // PreloadManager.Cache.GetMaterial("res://xxx.tres") // 如果需要自定义材质
        );
    }
}

