using HarmonyLib;
using System.IO;
using System.Linq;
using UnityEngine;

class SavePathPatch
{
    internal static string MESSAGE = "Patch To Override Utils GetSaveDataPath";
    // Disables Edge Of World Killing and pushing you out of map
    [HarmonyPatch(typeof(Utils), "GetSaveDataPath")]
    [HarmonyPrefix]
    private static bool Prefix_GetSaveDataPath(ref string __result)
    {
        if (File.Exists(Base.ModFolder + $"\\SavePath.txt"))
        {
            var fileLines = File.ReadAllLines(Base.ModFolder + $"\\SavePath.txt").ToList();
            if (fileLines.Count == 1)
            {
                if (Base.IsValidPath(fileLines[0]))
                {
                    __result = fileLines[0];
                    return false;
                }
            }
        }
        else 
        {
            File.WriteAllText(Base.ModFolder + $"\\SavePath.txt", Application.persistentDataPath);
        }
        return true;
    }
}