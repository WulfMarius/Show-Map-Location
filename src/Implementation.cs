using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ShowMapLocation
{
    public class Implementation
    {
        public const string NAME = "Show-Map-Location";

        private const string LOCATION_NAME = "GAMEPLAY_Location";

        private static MapDetail detail;

        public static void OnLoad()
        {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            Log("Version " + assemblyName.Version);
        }

        internal static void CleanupLocationMarkers()
        {
            Dictionary<string, List<MapElementSaveData>> m_MapElementData = Traverse.Create(InterfaceManager.m_Panel_Map).Field("m_MapElementData").GetValue<Dictionary<string, List<MapElementSaveData>>>();
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
                    Log("Removed {0} orphaned location markers", count);
                }
            }
        }

        internal static void HideLocationMarker()
        {
            if (detail == null)
            {
                return;
            }

            InterfaceManager.m_Panel_Map.RemoveMapDetailFromMap(detail);
        }

        internal static void Log(string message)
        {
            Debug.LogFormat("[" + NAME + "] {0}", message);
        }

        internal static void Log(string message, params object[] parameters)
        {
            string preformattedMessage = string.Format("[" + NAME + "] {0}", message);
            Debug.LogFormat(preformattedMessage, parameters);
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