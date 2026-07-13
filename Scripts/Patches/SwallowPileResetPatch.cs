using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Patches;

[HarmonyPatch(typeof(CombatManager), nameof(CombatManager.Reset))]
public static class SwallowPileResetPatch
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        SwallowPile.ResetForNewCombat();
    }
}