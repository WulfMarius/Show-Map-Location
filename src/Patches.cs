using Harmony;

namespace ShowMapLocation
{
    [HarmonyPatch(typeof(Panel_Map), "ResetToNormal")]
    internal class Panel_Map_ResetToNormal
    {
        internal static void Prefix(Panel_Map __instance, ref int opts)
        {
            if (GameManager.IsStoryMode())
            {
                return;
            }

            string mapName = Traverse.Create(__instance).Method("GetMapNameOfCurrentScene").GetValue<string>();
            bool canBeMapped = SceneCanBeMapped(mapName);
            if (!canBeMapped)
            {
                return;
            }

            int currentIndex = Traverse.Create(__instance).Method("GetIndexOfCurrentScene").GetValue<int>();
            int selectedIndex = Traverse.Create(__instance).Field("m_RegionSelectedIndex").GetValue<int>();
            if (currentIndex == selectedIndex)
            {
                opts |= 4;
            }
        }

        private static bool SceneCanBeMapped(string sceneName)
        {
            return RegionManager.SceneIsRegion(sceneName) || sceneName == "DamRiverTransitionZoneB" || (sceneName == "HighwayTransitionZone" || sceneName == "RavineTransitionZone") || sceneName == "MountainTownRegionSandbox";
        }
    }
}