using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Saves;

namespace TheInsatiable.Scripts;

[HarmonyPatch(typeof(NGeneralStatsGrid))]
    public static class GeneralStatsGridPatch
    {
        private static System.Reflection.MethodInfo _createCharacterSectionMethod;
        [HarmonyPostfix]
        [HarmonyPatch(nameof(NGeneralStatsGrid.LoadStats))]
        public static void AddCustomCharacter(NGeneralStatsGrid __instance)
        {
            if (_createCharacterSectionMethod == null)
            {
                _createCharacterSectionMethod = typeof(NGeneralStatsGrid).GetMethod("CreateCharacterSection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            }
            var progressSave = SaveManager.Instance?.Progress;
            if (progressSave == null) return;
            var Insatiable = ModelDb.Character<InsatiableCharacter>();

            if (Insatiable != null)
            {
                _createCharacterSectionMethod.Invoke(__instance, new object[] { progressSave, Insatiable.Id });
            }
        }
    }