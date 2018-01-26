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
        public static void Prefix(Panel_Map __instance, string sceneName)
        {
            var currentScene = NormalizeSceneName(GameManager.m_ActiveScene);
            if (currentScene == sceneName)
            {
                ShowMapLocation.ShowLocationMarker(sceneName);
            }
        }

        private static string NormalizeSceneName(string sceneName)
        {
            if (!GameManager.IsStoryMode() && sceneName == "MountainTownRegion")
            {
                return "MountainTownRegionSandbox";
            }

            return sceneName;
        }
    }
}