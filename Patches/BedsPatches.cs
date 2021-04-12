using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch]
class BedsPatches
{
// Disables check for nearby enemies
  [HarmonyPatch(typeof(Bed), "CheckEnemies")]
  [HarmonyPrefix]
  private static void Bed_CheckEnemies(ref bool __result)
  {
    __result = true;
  }
}
