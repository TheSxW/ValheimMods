using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

[BepInPlugin(GUID, MODNAME, VERSION)]
public class Base : BaseUnityPlugin
{
  public Base() {
    Instance = this;
    config = base.Config;
    Log = base.Logger;
    Harmony = new Harmony(GUID);
    Assembly = Assembly.GetExecutingAssembly();
    ModFolder = Path.GetDirectoryName(Assembly.Location);

    Patch<Patches.BedsPatches>();
  }
  #region Patching Method
  private static FieldInfo fi;
  internal static void Patch<T>()
  {
    // try to get internal static string MESSAGE; variable to display it as patch is loading
    fi = typeof(T).GetField("MESSAGE", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    if (fi != null) {
      string Message = (string)fi.GetValue(null);
      Log.LogInfo($"[{BasePatcher.MODNAME}] {Message}");
    }
    // now we executing our patch class specified in T
    Harmony.PatchAll(typeof(T));
  }
  #endregion
  #region Plugin Information
  public const string MODNAME = "modname";
  public const string AUTHOR = "TheSxW";
  public const string GUID = "TheSxW_modname";
  public const string VERSION = "1.0";
  #endregion
  #region Static Plugin Data
  internal static ConfigFile config;
  internal static BepInEx.Logging.ManualLogSource Log;
  internal static Harmony Harmony;
  internal static Assembly Assembly;
  internal static string ModFolder;
  internal static BasePatcher Instance;
  #endregion
}
