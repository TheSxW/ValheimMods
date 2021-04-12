using HarmonyLib;
[HarmonyPatch]
class UnrestrictedPortals
{
  [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
  [HarmonyPostfix]
  private static void Postfix(ref bool __result)
  {
    __result = true;
  }
}
