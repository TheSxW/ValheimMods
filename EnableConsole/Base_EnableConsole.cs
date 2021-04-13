using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

public class Base_EnableConsole : BaseUnityPlugin
{
    public Base_EnableConsole()
    {
        base.Logger.LogInfo("Enabled Console [F5]");
        Console.SetConsoleEnabled(true);
    }

    #region Plugin Information
    public const string MODNAME = "EnableConsole";
    public const string AUTHOR = "TheSxW";
    public const string GUID = "TheSxW_EnableConsole";
    public const string VERSION = "1.0";
    #endregion
}
