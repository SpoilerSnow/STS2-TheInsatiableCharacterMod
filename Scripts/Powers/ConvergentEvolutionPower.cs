using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class ConvergentEvolutionPower : InsatiablePowerModel
{
	private class Data
	{
		public readonly Dictionary<PowerModel, decimal> pending = new();
	}

	private const string _targetPlayerKey = "TargetPlayer";

	private Player? _playerTarget;

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	public Player PlayerTarget
	{
		get
		{
			return _playerTarget ?? throw new InvalidOperationException();
		}
		set
		{
			AssertMutable();
			_playerTarget = value;
			((StringVar)base.DynamicVars["TargetPlayer"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, _playerTarget.NetId);
		}
	}

	protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("TargetPlayer")];

	protected override object InitInternalData()
	{
		return new Data();
	}
	public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
	{
		// PlayerTarget 尚未设置（首次施加时 Hook 全局广播到未初始化的实例），跳过
		if (_playerTarget == null)
		{
			return Task.CompletedTask;
		}

		// 用 target（接收者）而非 power.Owner 判断——首次施加时 mutable Owner 为 null
		// 排除自身类型，防止双方趋同进化互相复制导致连锁
		// 只在目标玩家获得正面 Buff 层数时触发
		if (!target.IsPlayer || target.Player != _playerTarget
			|| power.Type != PowerType.Buff || amount <= 0m
			|| power is ConvergentEvolutionPower)
		{
			return Task.CompletedTask;
		}

		GetInternalData<Data>().pending[power] = amount;
		return Task.CompletedTask;
	}

	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		var data = GetInternalData<Data>();
		if (data.pending.Remove(power))
		{
			// 经过 ModifyPowerAmountGiven/Received 修正后实际增量可能已变化，
			// 确保最终增量仍为正值才复制
			if (amount <= 0m)
			{
				return;
			}

			Flash();

			// 消耗本 Power 一层
			await PowerCmd.Decrement(this);

			// 从 ModelDb 获取该 Power 的规范实例，创建可变副本，应用到自身
			PowerModel canonical = ModelDb.DebugPower(power.GetType());
			PowerModel mutableCopy = canonical.ToMutable();
			await PowerCmd.Apply(choiceContext, mutableCopy, base.Owner, amount, base.Owner, null);
		}
	}
}