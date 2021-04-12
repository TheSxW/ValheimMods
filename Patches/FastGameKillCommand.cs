using HarmonyLib;

class FastGameKillCommand
{
  [HarmonyPatch(typeof(Console), "InputText")]
  [HarmonyPrefix]
  private static bool InputText_Patch(Console __instance)
  {
    var command = __instance?.m_input?.text?.ToLower();
    if(command == null) return;
    if(command == "quit" || command == "shutdown")
    {
      System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
  }
}
