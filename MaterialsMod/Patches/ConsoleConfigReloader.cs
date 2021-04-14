using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ConsoleConfigReloader
{
    [HarmonyPatch(typeof(Console), "InputText")]
    [HarmonyPostfix]
    private static void InputText_Patch(Console __instance)
    {
        var command = __instance?.m_input?.text?.ToLower();
        if (command == null) return;
        if (command == "materials reload")
        {
            Base.config.Reload();
        }
    }
}