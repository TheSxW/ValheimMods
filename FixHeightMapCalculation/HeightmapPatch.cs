using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

class HeightmapPatch
{
    internal static string MESSAGE = "Attempt of Fixing Heightmap Lags.";
    
    [HarmonyPatch(typeof(Heightmap), "PokeHeightmaps")]
    [HarmonyPrefix]
    private static bool Prefix_PokeHeightmaps(TerrainModifier _instance, bool ___m_triggerOnPlaced)
    {
        if (_instance.gameObject.activeSelf && _instance.enabled && !coroutineStarted)
        {
            _instance.StartCoroutine(PokeHeightmaps_AsCoroutine(_instance, ___m_triggerOnPlaced));
        }
        return false;
    }
    internal static bool coroutineStarted = false;
    internal static IEnumerator PokeHeightmaps_AsCoroutine(TerrainModifier _instance, bool ___m_triggerOnPlaced)
    {
        coroutineStarted = true;
        var list = Heightmap.GetAllHeightmaps().Where(heightmap => heightmap.TerrainVSModifier(_instance)).ToList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Poke(!___m_triggerOnPlaced);
        }
        if (ClutterSystem.instance)
        {
            ClutterSystem.instance.ResetGrass(_instance.transform.position, _instance.GetRadius());
        }
        coroutineStarted = false;
        yield break;
    }

    // Disables Edge Of World Killing and pushing you out of map
    [HarmonyPatch(typeof(Heightmap), "Poke")]
    [HarmonyPrefix]
    private static bool Prefix_Poke(Heightmap _instance, bool delayed)
    {
        _instance.Regenerate();
        return false;
    }
    static MethodInfo mi_ApplyModifier = null;
    [HarmonyPatch(typeof(Heightmap), "ApplyModifiers")]
    [HarmonyPrefix]
    private static bool Prefix_ApplyModifiers(Heightmap _instance, float[] ___m_heights)
    {
        var ListToProcess = TerrainModifier.GetAllInstances().Where(tmod => tmod.enabled && IsInRadius(tmod, _instance)).ToList();
        for (int i = 0; i < ListToProcess.Count; i++)
        {
            if (ListToProcess[i].m_playerModifiction)
            {
                if (mi_ApplyModifier == null)
                    mi_ApplyModifier = typeof(Heightmap).GetMethod("ApplyModifier", BindingFlags.NonPublic | BindingFlags.Instance);
                mi_ApplyModifier.Invoke(_instance, new object[] { ListToProcess[i], ___m_heights.ToArray(), ___m_heights.ToArray() });
                //_instance.ApplyModifier(ListToProcess[i], ___m_heights.ToArray(), ___m_heights.ToArray());
            }
        }
        return false;
        // ORIGINAL
        //List<TerrainModifier> allInstances = TerrainModifier.GetAllInstances();
        //float[] array = null;
        //float[] levelOnly = null;
        //foreach (TerrainModifier terrainModifier in allInstances)
        //{
        //    if (terrainModifier.enabled && this.TerrainVSModifier(terrainModifier))
        //    {
        //        if (terrainModifier.m_playerModifiction && array == null)
        //        {
        //            array = this.m_heights.ToArray();
        //            levelOnly = this.m_heights.ToArray();
        //        }
        //        this.ApplyModifier(terrainModifier, array, levelOnly);
        //    }
        //}
        //this.m_clearedMask.Apply();
    }

    private static bool IsInRadius(TerrainModifier modifier, Heightmap _instance) {
        float Radius = modifier.GetRadius() + 4f;
        float Half_Width = (float)_instance.m_width * _instance.m_scale * 0.5f;
        return modifier.transform.position.x + Radius >= _instance.transform.position.x - Half_Width &&
            modifier.transform.position.x - Radius <= _instance.transform.position.x + Half_Width &&
            modifier.transform.position.z + Radius >= _instance.transform.position.z - Half_Width &&
            modifier.transform.position.z - Radius <= _instance.transform.position.z + Half_Width;
    }
    [HarmonyPatch(typeof(Heightmap), "Regenerate")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Prefix_Regenerate(IEnumerable<CodeInstruction> instructions)
    {
        // ORIGINAL 
        /*
        0   0000    ldarg.0
        1   0001    call instance bool Heightmap::HaveQueuedRebuild()
        2   0006    brfalse.s   6(0013) ldarg.0
        3   0008    ldarg.0
        4   0009    ldstr   "Regenerate"
        5   000E    call    instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::CancelInvoke(string)
        6   0013    ldarg.0
        7   0014    call    instance void Heightmap::Generate()
        8   0019    ldarg.0
        9   001A    call    instance void Heightmap::RebuildCollisionMesh()
        10  001F    ldarg.0
        11  0020    call    instance void Heightmap::UpdateCornerDepths()
        12  0025    ldarg.0
        13  0026    ldc.i4.1
        14  0027    stfld   bool Heightmap::m_dirty
        15  002C ret
        */
        // AFTER
        /*
        0	0000	ldarg.0
        1	0001	call	instance void Heightmap::Generate()
        2	0006	ldarg.0
        3	0007	call	instance void Heightmap::RebuildCollisionMesh()
        4	000C	ldarg.0
        5	000D	call	instance void Heightmap::UpdateCornerDepths()
        6	0012	ldarg.0
        7	0013	ldc.i4.1
        8	0014	stfld	bool Heightmap::m_dirty
        9	0019	ret
         */

        //remove first 6
        int remove_till = 6;
        int count_instructions = 0;
        List<CodeInstruction> newInstructions = new List<CodeInstruction>();
        foreach (var instruction in instructions)
        {
            // skip first 0-5 instructions and start from id 6
            if (count_instructions >= remove_till) {
                yield return instruction;
                //newInstructions.Add(instruction);
            }
            count_instructions++;
        }

        //return newInstructions.AsEnumerable();
    }
}
