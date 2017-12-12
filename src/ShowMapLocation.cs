using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace ShowMapLocation
{
    internal class ShowMapLocation
    {
        private static GameObject locationMarker;

        internal static void HideLocationMarker()
        {
            if (locationMarker == null)
            {
                return;
            }

            locationMarker.transform.parent = null;
            locationMarker.SetActive(false);
        }

        internal static void ShowLocationMarker(string sceneName)
        {
            Traverse traverse = Traverse.Create(InterfaceManager.m_Panel_Map);
            Vector3 mapPosition = GetWorldPosition(traverse, sceneName);

            MapElementSaveData mapElementSaveData = new MapElementSaveData();
            mapElementSaveData.m_LocationNameLocID = "GAMEPLAY_Location";
            mapElementSaveData.m_SpriteName = "ico_X";
            mapElementSaveData.m_BigSprite = false;
            mapElementSaveData.m_IsDetail = true;
            mapElementSaveData.m_NameIsKnown = true;
            mapElementSaveData.m_PositionOnMap = mapPosition;
            mapElementSaveData.m_ActiveMissionLocIDs = new List<string>();
            mapElementSaveData.m_ActiveMissionTimerIDs = new List<string>();
            mapElementSaveData.m_ActiveMissionIDs = new List<string>();

            if (locationMarker == null)
            {
                InitializeLocationMarker(InterfaceManager.m_Panel_Map);
            }
            locationMarker.GetComponent<MapIcon>().DoSetup(mapElementSaveData, InterfaceManager.m_Panel_Map.m_DetailEntryActiveObjects, 1000, MapIcon.MapIconType.TopIcon);

            AddToMapData(traverse, mapElementSaveData);
        }

        private static void AddToMapData(Traverse traverse, MapElementSaveData mapElementSaveData)
        {
            Dictionary<Transform, MapElementSaveData> m_TransformToMapData = traverse.Field("m_TransformToMapData").GetValue<Dictionary<Transform, MapElementSaveData>>();
            m_TransformToMapData.Add(locationMarker.transform, mapElementSaveData);
        }

        private static Vector3 GetWorldPosition(Traverse traverse, string sceneName)
        {
            Traverse worldPositionToMapPosition = traverse.Method("WorldPositionToMapPosition", new System.Type[] { typeof(string), typeof(Vector3) });
            return worldPositionToMapPosition.GetValue<Vector3>(new object[] { sceneName, GameManager.GetPlayerTransform().localPosition });
        }

        private static void InitializeLocationMarker(Panel_Map panelMap)
        {
            locationMarker = Object.Instantiate<GameObject>(panelMap.m_DetailEntryPrefab);
            locationMarker.transform.localScale = Vector3.one;
        }
    }
}