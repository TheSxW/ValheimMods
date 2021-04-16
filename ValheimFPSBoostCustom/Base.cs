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
        Harmony = new Harmony(GUID);
        Assembly = Assembly.GetExecutingAssembly();
        ModFolder = Path.GetDirectoryName(Assembly.Location);

        CustomSettingsAsTextFile();
    }

    internal void CustomSettingsAsTextFile()
    {
        if (File.Exists(Base.ModFolder + $"\\Settings.txt"))
        {
            // load saved values
            var fileLines = File.ReadAllLines(Base.ModFolder + $"\\Settings.txt").ToList();
            for (int i = 0; i < fileLines.Count; i++) {

                string[] splitted = fileLines[i].Split('=');
                if (splitted.Length == 2)
                {
                    switch (splitted[0])
                    {
                        case "patchPostProcessor":
                            if (int.Parse(splitted[1]) == 1)
                            {
                                Patch<PatchPostProcessing>();
                            }
                            break;
                        case "antiAliasing":
                            QualitySettings.antiAliasing = int.Parse(splitted[1]); break;
                        case "anisotropicFiltering":
                            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)(int)Mathf.Clamp(int.Parse(splitted[1]), 0f, 2f); break;
                        case "lodBias":
                            QualitySettings.lodBias = float.Parse(splitted[1]); break;
                        case "masterTextureLimit":
                            QualitySettings.masterTextureLimit = int.Parse(splitted[1]); break;
                        case "maximumLODLevel":
                            QualitySettings.maximumLODLevel = int.Parse(splitted[1]); break;
                        case "maxQueuedFrames":
                            QualitySettings.maxQueuedFrames = int.Parse(splitted[1]); break;
                        case "particleRaycastBudget":
                            QualitySettings.particleRaycastBudget = int.Parse(splitted[1]); break;
                        case "pixelLightCount":
                            QualitySettings.pixelLightCount = int.Parse(splitted[1]); break;
                        case "realtimeReflectionProbes":
                            QualitySettings.realtimeReflectionProbes = bool.Parse(splitted[1]); break;
                        case "shadowResolution":
                            QualitySettings.shadowResolution = (ShadowResolution)(int)Mathf.Clamp(int.Parse(splitted[1]), 0f, 3f); break;
                        case "shadowCascades":
                            QualitySettings.shadowCascades = int.Parse(splitted[1]); break;
                        case "shadowCascade2Split":
                            QualitySettings.shadowCascade2Split = float.Parse(splitted[1]); break;
                        case "shadowDistance":
                            QualitySettings.shadowDistance = float.Parse(splitted[1]); break;
                        case "shadows":
                            QualitySettings.shadows = (ShadowQuality)(int)Mathf.Clamp(int.Parse(splitted[1]), 0f, 2f); break;
                        case "skinWeights":
                            var parsed = int.Parse(splitted[1]);
                            if (parsed == 1 || parsed == 2 || parsed == 4 || parsed == 255) {
                                QualitySettings.skinWeights = (SkinWeights)parsed;
                            }
                            break;
                        case "softParticles":
                            QualitySettings.softParticles = bool.Parse(splitted[1]); break;
                        case "softVegetation":
                            QualitySettings.softVegetation = bool.Parse(splitted[1]); break;
                        case "streamingMipmapsActive":
                            QualitySettings.streamingMipmapsActive = bool.Parse(splitted[1]); break;
                        case "streamingMipmapsAddAllCameras":
                            QualitySettings.streamingMipmapsAddAllCameras = bool.Parse(splitted[1]); break;
                        case "streamingMipmapsMaxFileIORequests":
                            QualitySettings.streamingMipmapsMaxFileIORequests = int.Parse(splitted[1]); break;
                        case "streamingMipmapsMaxLevelReduction":
                            QualitySettings.streamingMipmapsMaxLevelReduction = int.Parse(splitted[1]); break;
                        case "streamingMipmapsMemoryBudget":
                            QualitySettings.streamingMipmapsMemoryBudget = float.Parse(splitted[1]); break;
                        case "vSyncCount":
                            QualitySettings.vSyncCount = int.Parse(splitted[1]); break;
                    }
                }
            }
        }
        else
        {
            // save to file a current values
            string fileData = "";
            fileData += "patchPostProcessor=0\n\r";
            fileData += "antiAliasing=" + QualitySettings.antiAliasing + "\n\r";
            fileData += "anisotropicFiltering=" + (int)QualitySettings.anisotropicFiltering + "\n\r";
            fileData += "lodBias=" + QualitySettings.lodBias + "\n\r";
            fileData += "masterTextureLimit=" + QualitySettings.masterTextureLimit + "\n\r";
            fileData += "maximumLODLevel=" + QualitySettings.maximumLODLevel + "\n\r";
            fileData += "maxQueuedFrames=" + QualitySettings.maxQueuedFrames + "\n\r";
            fileData += "particleRaycastBudget=" + QualitySettings.particleRaycastBudget + "\n\r";
            fileData += "pixelLightCount=" + QualitySettings.pixelLightCount + "\n\r";
            fileData += "realtimeReflectionProbes=" + QualitySettings.realtimeReflectionProbes + "\n\r";
            fileData += "shadowResolution=" + (int)QualitySettings.shadowResolution + "\n\r";
            fileData += "shadowCascades=" + QualitySettings.shadowCascades + "\n\r";
            fileData += "shadowCascade2Split=" + QualitySettings.shadowCascade2Split + "\n\r";
            fileData += "shadowDistance=" + QualitySettings.shadowDistance + "\n\r";
            fileData += "shadows=" + (int)QualitySettings.shadows + "\n\r";
            fileData += "skinWeights=" + (int)QualitySettings.skinWeights + "\n\r";
            fileData += "softParticles=" + QualitySettings.softParticles + "\n\r";
            fileData += "softVegetation=" + QualitySettings.softVegetation + "\n\r";
            fileData += "streamingMipmapsActive=" + QualitySettings.streamingMipmapsActive + "\n\r";
            fileData += "streamingMipmapsAddAllCameras=" + QualitySettings.streamingMipmapsAddAllCameras + "\n\r";
            fileData += "streamingMipmapsMaxFileIORequests=" + QualitySettings.streamingMipmapsMaxFileIORequests + "\n\r";
            fileData += "streamingMipmapsMaxLevelReduction=" + QualitySettings.streamingMipmapsMaxLevelReduction + "\n\r";
            fileData += "streamingMipmapsMemoryBudget=" + QualitySettings.streamingMipmapsMemoryBudget + "\n\r";
            fileData += "vSyncCount=" + QualitySettings.vSyncCount + "\n\r";

            File.WriteAllText(Base.ModFolder + $"\\SavePath.txt", fileData);
        }
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
    public const string MODNAME = "ValheimFPSBoostCustom";
    public const string AUTHOR = "TheSxW";
    public const string GUID = "TheSxW_ValheimFPSBoostCustom";
    public const string VERSION = "1.0.0";
    #endregion
    #region Static Plugin Data
    internal static ConfigFile config;
    internal static BepInEx.Logging.ManualLogSource Log;
    internal static Harmony Harmony;
    internal static Assembly Assembly;
    internal static string ModFolder;
    #endregion

}