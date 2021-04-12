using HarmonyLib;

class EnableConsole
{
  // enables console then main menu shows up
  [HarmonyPatch(typeof(MenuScene), "Awake")]
  [HarmonyPostfix]
  private static void MenuScene_Awake()
  {
    Console.SetConsoleEnabled(true);
  }
}
