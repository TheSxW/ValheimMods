using HarmonyLib;

[HarmonyPatch]
class BedsPatches
{
    internal static string MESSAGE = "Disabling nearby enemy check";
    // Disables check for nearby enemies
    [HarmonyPatch(typeof(Bed), "CheckEnemies")]
    [HarmonyPrefix]
    private static void Bed_CheckEnemies(ref bool __result)
    {
        __result = true;
    }
}