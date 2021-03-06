using System;
using System.IO;
using System.Linq;
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
        //Harmony = new Harmony(GUID);
        Assembly = Assembly.GetExecutingAssembly();
        ModFolder = Path.GetDirectoryName(Assembly.Location);
        ChangeSavePath();
    }
    private string GameDir = "GAMEDIR_";
    private string ModDir = "MODDIR_";
    internal void ChangeSavePath() {
        if (File.Exists(Base.ModFolder + $"\\SavePath.txt"))
        {
            var fileLines = File.ReadAllLines(Base.ModFolder + $"\\SavePath.txt").ToList();
            if (fileLines.Count == 1)
            {
                string StartingPath = "";
                string restOfPath = "";
                if (fileLines[0].StartsWith(GameDir)) {
                    StartingPath = Application.dataPath;
                    restOfPath = fileLines[0].Substring(GameDir.Length);
                }
                if (fileLines[0].StartsWith(ModDir)) {
                    StartingPath = Base.ModFolder;
                    restOfPath = fileLines[0].Substring(ModDir.Length);
                }

                if (restOfPath.StartsWith("\\"))
                    restOfPath = restOfPath.Substring(2);

                string endPath = Path.Combine(StartingPath, restOfPath);
                if (IsValidPath(endPath))
                {
                    Log.LogInfo($"﻿Successfully Changed Save Path: {Path.GetFullPath((new Uri(endPath)).LocalPath)}");
                    Utils.SetSaveDataPath(endPath);
                    return;
                }
            }
        }
        else
        {
            File.WriteAllText(Base.ModFolder + $"\\SavePath.txt", Application.persistentDataPath);
        }
    }
    #region Patching Method
    //private static FieldInfo fi;
    //internal static void Patch<T>()
    //{
    //    // try to get internal static string MESSAGE; variable to display it as patch is loading
    //    fi = typeof(T).GetField("MESSAGE", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    //    if (fi != null)
    //    {
    //        string Message = (string)fi.GetValue(null);
    //        Log.LogInfo($"[{MODNAME}] {Message}");
    //    }
    //    // now we executing our patch class specified in T
    //    Harmony.PatchAll(typeof(T));
    //}
    #endregion
    #region Plugin Information
    public const string MODNAME = "SaveWhereYouWant";
    public const string AUTHOR = "TheSxW";
    public const string GUID = "TheSxW_SaveWhereYouWant";
    public const string VERSION = "1.1";
    #endregion
    #region Static Plugin Data
    internal static ConfigFile config;
    internal static BepInEx.Logging.ManualLogSource Log;
    //internal static Harmony Harmony;
    internal static Assembly Assembly;
    internal static string ModFolder;
    #endregion

    #region Help Functions
    internal static bool IsValidPath(string path, bool allowRelativePaths = false)
    {
        try
        {
            // test if it can parse it
            Path.GetFullPath(path);

            if (allowRelativePaths)
            {
                return Path.IsPathRooted(path);
            }
            else
            {
                string root = Path.GetPathRoot(path);
                return string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion
}