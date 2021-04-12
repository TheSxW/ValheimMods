using HarmonyLib;

class NoEdgeMapKill
{
    // Disables Edge Of World Killing and pushing you out of map
    [HarmonyPatch(typeof(Player), "EdgeOfWorldKill")]
    [HarmonyPrefix]
    private static bool Prefix()
    {
        return false;
    }
}
