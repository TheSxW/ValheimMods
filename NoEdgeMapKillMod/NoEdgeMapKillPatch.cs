using HarmonyLib;
class NoEdgeMapKillPatch
{
    internal static string MESSAGE = "Disabling killing and pushing out of edge of map.";
    // Disables Edge Of World Killing and pushing you out of map
    [HarmonyPatch(typeof(Player), "EdgeOfWorldKill")]
    [HarmonyPrefix]
    private static bool Prefix()
    {
        return false;
    }
}
