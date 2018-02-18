using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace ShowMapLocation
{
    internal class ShowMapLocation
    {
        private const string LOCATION_NAME = "GAMEPLAY_Location";

        private static MapDetail detail;

        public static void OnLoad()
        {
            Debug.Log("[Show-Map-Location]: Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }

        internal static void HideLocationMarker()
        {
            if (detail == null)
            {
                return;
            }

            InterfaceManager.m_Panel_Map.RemoveMapDetailFromMap(detail);
        }

        internal static void CleanupLocationMarkers()
        {
            Dictionary<string, List<MapElementSaveData>> m_MapElementData = AccessTools.Field(typeof(Panel_Map), "m_MapElementData").GetValue(InterfaceManager.m_Panel_Map) as Dictionary<string, List<MapElementSaveData>>;
            if (m_MapElementData == null)
            {
                return;
            }

            foreach (List<MapElementSaveData> eachCollection in m_MapElementData.Values)
            {
                if (eachCollection == null)
                {
                    continue;
                }

                int count = eachCollection.RemoveAll(mapElement => mapElement.m_LocationNameLocID == LOCATION_NAME);
                if (count > 0)
                {
                    Debug.Log("Removed " + count + " orphaned location markers");
                }
            }
        }

        internal static void ShowLocationMarker()
        {
            if (detail == null)
            {
                GameObject gameObject = new GameObject();
                detail = gameObject.AddComponent<MapDetail>();
                detail.m_LocID = LOCATION_NAME;
                detail.m_SpriteName = "ico_X";
                detail.m_IconType = MapIcon.MapIconType.TopIcon;
            }

            detail.transform.position = GameManager.GetPlayerTransform().position;
            InterfaceManager.m_Panel_Map.AddMapDetailToMap(detail);
        }
    }
}