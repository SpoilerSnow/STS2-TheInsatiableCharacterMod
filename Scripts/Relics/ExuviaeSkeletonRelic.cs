using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableRelicPool))]
public class ExuviaeSkeletonRelic : InsatiableRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    
    // 记录本场战斗是否已经消耗了“第一次”的机会
    private bool _hasTriggeredThisCombat;

    /// <summary>
    /// 战斗开始时重置状态，确保“每场战斗”生效一次
    /// </summary>
    public override Task BeforeCombatStart()
    {
        _hasTriggeredThisCombat = false;
        return base.BeforeCombatStart();
    }

    /// <summary>
    /// 核心：设置伤害上限 (Cap)
    /// </summary>
    public override decimal ModifyDamageCap(Creature? target, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // 1. 如果已经触发过，不限制
        // 2. 目标不是自己，不限制
        // 3. 没有攻击者(dealer)或不是攻击伤害，不限制
        if (_hasTriggeredThisCombat || 
            target != base.Owner?.Creature || 
            dealer == null || 
            !props.IsPoweredAttack())
        {
            return decimal.MaxValue; // 不设置上限
        }
        
        // 将伤害上限锁死在 9
        // 注意：如果原始伤害 <= 9，引擎不会认为伤害被修改，后续的 AfterModifyingDamageAmount 就不会被触发
        return 9m;
    }

    /// <summary>
    /// 核心：当且仅当伤害【实际被截断】(即原始伤害 > 9) 时，引擎才会调用此方法
    /// </summary>
    public override Task AfterModifyingDamageAmount(CardModel? cardSource)
    {
        if (!_hasTriggeredThisCombat)
        {
            _hasTriggeredThisCombat = true;
            Flash(); // 播放遗物生效的闪光特效
        }
        return Task.CompletedTask;
    }
}