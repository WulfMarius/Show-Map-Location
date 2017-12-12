using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShowMapLocation
{
    [HarmonyPatch(typeof(Panel_Map), "UnloadMapElements")]
    internal class Panel_Map_UnloadMapElements
    {
        public static void Prefix()
        {
            ShowMapLocation.HideLocationMarker();
        }
    }

    [HarmonyPatch(typeof(Panel_Map), "LoadMapElementsForScene")]
    internal class Panel_Map_LoadMapElementsForScene
    {
        public static void Postfix(Panel_Map __instance, string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                ShowMapLocation.ShowLocationMarker(sceneName);
            }
        }
    }
}