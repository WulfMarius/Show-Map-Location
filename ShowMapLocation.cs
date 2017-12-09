using System;
using System.Collections.Generic;

using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShowMapLocation
{
    [HarmonyPatch(typeof(Panel_Map))]
    [HarmonyPatch("LoadMapElementsForScene")]
    public class ShowMapLocation
    {
        public static void Postfix(Panel_Map __instance, String sceneName)
        {
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                // not the current region -> don't add the current location to the map
                return;
            }

            if (__instance.m_DetailEntryPoolParent.childCount <= 0)
            {
                Debug.Log("Could not create map location because m_DetailEntryPoolParent has no children");
                return;
            }

            Traverse panel_Map = Traverse.Create(__instance);
            Traverse worldPositionToMapPosition = panel_Map.Method("WorldPositionToMapPosition", new Type[] { typeof(String), typeof(Vector3) });
            Vector3 mapPosition = worldPositionToMapPosition.GetValue<Vector3>(new object[] { sceneName, GameManager.GetPlayerTransform().localPosition });

            MapElementSaveData mapElementSaveData = new MapElementSaveData();
            mapElementSaveData.m_LocationNameLocID = "GAMEPLAY_Location";
            mapElementSaveData.m_SpriteName = "ico_X";
            mapElementSaveData.m_BigSprite = false;
            mapElementSaveData.m_IsDetail = true;
            mapElementSaveData.m_NameIsKnown = true;
            mapElementSaveData.m_PositionOnMap = (Vector2)mapPosition;
            mapElementSaveData.m_ActiveMissionLocIDs = new List<string>();
            mapElementSaveData.m_ActiveMissionTimerIDs = new List<string>();
            mapElementSaveData.m_ActiveMissionIDs = new List<string>();

            Transform child = __instance.m_DetailEntryPoolParent.GetChild(0);
            child.GetComponent<MapIcon>().DoSetup(mapElementSaveData, __instance.m_DetailEntryActiveObjects, 1000, MapIcon.MapIconType.TopIcon);

            Dictionary<Transform, MapElementSaveData> m_TransformToMapData = panel_Map.Field("m_TransformToMapData").GetValue<Dictionary<Transform, MapElementSaveData>>();
            m_TransformToMapData.Add(child, mapElementSaveData);
        }
    }
}
