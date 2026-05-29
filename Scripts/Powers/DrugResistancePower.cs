using System.Collections.Generic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInsatiable.Scripts;

public class DrugResistancePower : InsatiablePowerModel
{
    private readonly Dictionary<Creature, HashSet<Type>> _allowedDebuffTypesByOwner = new();
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        Creature owner = base.Owner;
        if (target != owner || canonicalPower.GetTypeForAmount(amount) != PowerType.Debuff || !canonicalPower.IsVisible)
        {
            modifiedAmount = amount;
            return false;
        }

        HashSet<Type> allowedDebuffs = _allowedDebuffTypesByOwner.TryGetValue(owner, out HashSet<Type>? set)
            ? set
            : (_allowedDebuffTypesByOwner[owner] = new HashSet<Type>());

        Type debuffType = canonicalPower.GetType();
        if (!allowedDebuffs.Contains(debuffType))
        {
            allowedDebuffs.Add(debuffType);
            modifiedAmount = amount;
            return false;
        }
        modifiedAmount = 0m;
        Flash();
        return true;
    }
    public override Task AfterCombatEnd(CombatRoom room)
    {
        _allowedDebuffTypesByOwner.Clear();
        return Task.CompletedTask;
    }
}