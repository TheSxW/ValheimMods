using HarmonyLib;
using UnityEngine;
using UnityEngine.PostProcessing;

class DisablePostProcessing
{
  [HarmonyPatch(typeof(PostProcessingBehaviour), "OnEnable")]
  [HarmonyPrefix]
  private static bool OnEnable(PostProcessingBehaviour __instance)
  {
    __instance.enabled = false;
    return false;
  }

  [HarmonyPatch(typeof(PostProcessingBehaviour), "OnDisable")]
  [HarmonyPrefix]
  private static bool OnDisable(PostProcessingBehaviour __instance)
  {
    foreach (PostProcessingBehaviour postProcessingBehaviour in Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>())
    {
      if (!postProcessingBehaviour.Equals(__instance))
      {
        postProcessingBehaviour.enabled = false;
        UnityEngine.Object.Destroy(postProcessingBehaviour);
      }
    }
    GraphicsUtils.Dispose();
    return false;
  }
}
