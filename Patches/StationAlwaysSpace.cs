using HarmonyLib;

class StationAlwaysSpace {
    [HarmonyPatch(typeof(StationExtension), "OtherExtensionInRange")]
    [HarmonyPrefix]
    private static void OtherExtensionInRange_PostFix(ref bool __result, ref bool __runOriginal) {
        __result = false;
        __runOriginal = false;
    }
}
