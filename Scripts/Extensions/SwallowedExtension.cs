using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;

public static class CombatHistorySwallowedExtensions
{
    public static void CardSwallowed(this CombatHistory history, ICombatState combatState, CardModel card)
    {
        // ✅ 正确：创建并添加 CardSwallowedEntry
        var entry = new CardSwallowedEntry(card, combatState.RoundNumber, combatState.CurrentSide, history, combatState.Players);
        var addMethod = typeof(CombatHistory).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
        addMethod?.Invoke(history, new object[] { combatState, entry });
    }
    
    public static void CreatureSwallowed(this CombatHistory history, ICombatState combatState, Creature creature)
    {
        // ✅ 正确：创建并添加 CreatureSwallowedEntry
        var entry = new CreatureSwallowedEntry(creature, combatState.RoundNumber, combatState.CurrentSide, history, combatState.Players);
        var addMethod = typeof(CombatHistory).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
        addMethod?.Invoke(history, new object[] { combatState, entry });
    }
}