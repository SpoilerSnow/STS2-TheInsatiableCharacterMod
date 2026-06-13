using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[HarmonyPatch(typeof(CardCmd))]
internal static class AfflictReturnPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CardCmd.Afflict), typeof(AfflictionModel), typeof(CardModel), typeof(decimal))]
    public static bool Prefix_Afflict(ref Task<AfflictionModel?> __result, AfflictionModel affliction, CardModel card, decimal amount)
    {
        Creature ownerCreature = card.Owner?.Creature;
        
        if (ownerCreature != null)
        {
            int weatheringPowerAmount = ownerCreature.GetPowerAmount<WeatheringPower>();
            
            if (weatheringPowerAmount > 0 && !(affliction is WeatheringAffliction))
            {
                CardCmd.ClearAffliction(card);
                __result = Task.FromResult<AfflictionModel?>(null);
                return false;
            }
        }
        return true;
    }
}