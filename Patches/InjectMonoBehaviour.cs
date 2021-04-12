using HarmonyLib;
using UnityEngine;
using System;

class InjectMonoBehaviour
{
  [HarmonyPatch(typeof(MenuScene), "Awake")]
  [HarmonyPostfix]
  private static void MenuScene_Awake()
  {
    UnityEngine.GameObject temporalGO = new UnityEngine.GameObject("Injected-" + Guid.NewGuid().ToString());
    temporalGO.AddComponent<MonoBehaviourScript>(); // adding our script to newly created game object
    MonoBehaviourScript.DontDestroyOnLoad(tmpGO); // using MonoBehaviourScript cause it should contain UnityEngine.MonoBehaviour structure which contains that method.
  }
}
class MonoBehaviourScript : MonoBehaviour
{
  void Awake() { }
  void Update() { }
  void OnGUI() { }
}
