using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

[BepInPlugin(GUID, MODNAME, VERSION)]
[BepInProcess("valheim.exe")]
public class Base : BaseUnityPlugin
{
    public Base()
    {
        config = base.Config;
        Log = base.Logger;
        Harmony = new Harmony(GUID);
        Assembly = Assembly.GetExecutingAssembly();
        ModFolder = Path.GetDirectoryName(Assembly.Location);
        config.Reload();
        Configuration.enable_old_calculation = config.Bind<bool>("Old Calculation", "Enable", false, "");
        //Wood = new MaterialSettings() { maxSupport = 100f, minSupport = 10f, verticalLoss = 0.125f, horizontalLoss = 0.2f };
        //Stone = new MaterialSettings() { maxSupport = 1000f, minSupport = 100f, verticalLoss = 0.125f, horizontalLoss = 0.1f };
        //Iron = new MaterialSettings() { maxSupport = 1500f, minSupport = 20f, verticalLoss = 0.07692308f, horizontalLoss = 0.07692308f };
        //HardWood = new MaterialSettings() { maxSupport = 140f, minSupport = 10f, verticalLoss = 0.1f, horizontalLoss = 0.166666672f };
        //Default = new MaterialSettings() { maxSupport = 0f, minSupport = 0f, verticalLoss = 0f, horizontalLoss = 0f };
        //if (___m_materialType == WearNTear.MaterialType.Wood)
        //{
        //    return false;
        //}
        //if (___m_materialType == WearNTear.MaterialType.Stone)
        //{
        //    maxSupport *= 10f;
        //    minSupport *= 10f;
        //    verticalLoss *= 0.5f;
        //    horizontalLoss *= 0.8f;
        //    return false;
        //}
        //if (___m_materialType == WearNTear.MaterialType.Iron)
        //{
        //    maxSupport *= 15f;
        //    minSupport *= 2f;
        //    verticalLoss *= 0.75f;
        //    horizontalLoss *= 0.5f;
        //    return false;
        //}
        //if (___m_materialType == WearNTear.MaterialType.HardWood)
        //{
        //    maxSupport *= 2f;
        //    //minSupport *= 10f;
        //    verticalLoss *= 0.5f;
        //    horizontalLoss *= 0.8f;
        //    return false;
        //}
        Configuration.Wood_maxSupport = config.Bind<float>("Wood", "Max Support (Old)", 100f, "default: 100");
        Configuration.Wood_minSupport = config.Bind<float>("Wood", "Min Support (Old)", 10f, "default: 10");
        Configuration.Wood_verticalLoss = config.Bind<float>("Wood", "Vertical Loss (Old)", 0.125f, "default: 0.125");
        Configuration.Wood_horizontalLoss = config.Bind<float>("Wood", "Horizontal Loss (Old)", 0.2f, "default: 0.2");
        Configuration.Wood_maxSupport_New = config.Bind<float>("Wood", "Max Support (New)", 100f, "Starting value");
        Configuration.Wood_minSupport_New = config.Bind<float>("Wood", "Min Support (New)", 10f, "Starting value");
        Configuration.Wood_verticalLoss_New = config.Bind<float>("Wood", "Vertical Loss (New)", 0.1f, "Starting value");
        Configuration.Wood_horizontalLoss_New = config.Bind<float>("Wood", "Horizontal Loss (New)", 0.125f, "Starting value");

        Configuration.Stone_maxSupport = config.Bind<float>("Stone", "Max Support (Old)", 1000f, "default: 1000");
        Configuration.Stone_minSupport = config.Bind<float>("Stone", "Min Support (Old)", 100f, "default: 100");
        Configuration.Stone_verticalLoss = config.Bind<float>("Stone", "Vertical Loss (Old)", 0.125f, "default: 0.125");
        Configuration.Stone_horizontalLoss = config.Bind<float>("Stone", "Horizontal Loss (Old)", 0.1f, "default: 0.1");
        Configuration.Stone_maxSupport_New = config.Bind<float>("Stone", "Max Support (New)", 10, "Multiply Starting Value");
        Configuration.Stone_minSupport_New = config.Bind<float>("Stone", "Min Support (New)", 10f, "Multiply Starting Value");
        Configuration.Stone_verticalLoss_New = config.Bind<float>("Stone", "Vertical Loss (New)", 0.5f, "Multiply Starting Value");
        Configuration.Stone_horizontalLoss_New = config.Bind<float>("Stone", "Horizontal Loss (New)", 0.8f, "Multiply Starting Value");

        Configuration.Iron_maxSupport = config.Bind<float>("Iron", "Max Support (Old)", 1500f, "default: 1500");
        Configuration.Iron_minSupport = config.Bind<float>("Iron", "Min Support (Old)", 20f, "default: 20");
        Configuration.Iron_verticalLoss = config.Bind<float>("Iron", "Vertical Loss (Old)", 0.07692308f, "default: 0.07692308");
        Configuration.Iron_horizontalLoss = config.Bind<float>("Iron", "Horizontal Loss (Old)", 0.07692308f, "default: 0.07692308");
        Configuration.Iron_maxSupport_New = config.Bind<float>("Iron", "Max Support (New)", 15f, "Multiply Starting Value");
        Configuration.Iron_minSupport_New = config.Bind<float>("Iron", "Min Support (New)", 2f, "Multiply Starting Value");
        Configuration.Iron_verticalLoss_New = config.Bind<float>("Iron", "Vertical Loss (New)", 0.75f, "Multiply Starting Value");
        Configuration.Iron_horizontalLoss_New = config.Bind<float>("Iron", "Horizontal Loss (New)", 0.5f, "Multiply Starting Value");

        Configuration.HardWood_maxSupport = config.Bind<float>("Hard Wood", "Max Support (Old)", 140f, "default: 140");
        Configuration.HardWood_minSupport = config.Bind<float>("Hard Wood", "Min Support (Old)", 10f, "default: 10");
        Configuration.HardWood_verticalLoss = config.Bind<float>("Hard Wood", "Vertical Loss (Old)", 0.1f, "default: 0.1");
        Configuration.HardWood_horizontalLoss = config.Bind<float>("Hard Wood", "Horizontal Loss (Old)", 0.166666672f, "default: 0.166666672");
        Configuration.HardWood_maxSupport_New = config.Bind<float>("Hard Wood", "Max Support (New)", 2f, "Multiply Starting Value");
        Configuration.HardWood_minSupport_New = config.Bind<float>("Hard Wood", "Min Support (New)", 1f, "Multiply Starting Value");
        Configuration.HardWood_verticalLoss_New = config.Bind<float>("Hard Wood", "Vertical Loss (New)", 0.5f, "Multiply Starting Value");
        Configuration.HardWood_horizontalLoss_New = config.Bind<float>("Hard Wood", "Horizontal Loss (New)", 0.8f, "Multiply Starting Value");


        Patch<ConsoleConfigReloader>();
        Patch<EditMaterialProperties>();
    }

    #region Patching Method
    private static FieldInfo fi;
    internal static void Patch<T>()
    {
        // try to get internal static string MESSAGE; variable to display it as patch is loading
        fi = typeof(T).GetField("MESSAGE", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (fi != null)
        {
            string Message = (string)fi.GetValue(null);
            Log.LogInfo($"[{MODNAME}] {Message}");
        }
        // now we executing our patch class specified in T
        Harmony.PatchAll(typeof(T));
    }
    #endregion
    #region Plugin Information
    public const string MODNAME = "EditMaterialProperties";
    public const string AUTHOR = "TheSxW";
    public const string GUID = "TheSxW_EditMaterialProperties";
    public const string VERSION = "1.0";
    #endregion
    #region Static Plugin Data
    internal static ConfigFile config;
    internal static BepInEx.Logging.ManualLogSource Log;
    internal static Harmony Harmony;
    internal static Assembly Assembly;
    internal static string ModFolder;
    #endregion
}
