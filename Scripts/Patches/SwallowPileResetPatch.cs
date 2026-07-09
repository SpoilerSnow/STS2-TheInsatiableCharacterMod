using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts;

[HarmonyPatch(typeof(CombatManager), "OnBattleEnd")]
public static class SwallowPileResetPatch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        SwallowPile.ResetForNewCombat();
    }
}