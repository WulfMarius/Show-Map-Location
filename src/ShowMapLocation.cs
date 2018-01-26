using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace ShowMapLocation
{
    internal class ShowMapLocation
    {
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

        internal static void ShowLocationMarker(string sceneName)
        {
            if (detail == null)
            {
                GameObject gameObject = new GameObject();
                detail = gameObject.AddComponent<MapDetail>();
                detail.m_LocID = "GAMEPLAY_Location";
                detail.m_SpriteName = "ico_X";
                detail.m_IconType = MapIcon.MapIconType.TopIcon;
            }

            detail.transform.position = GameManager.GetPlayerTransform().position;
            InterfaceManager.m_Panel_Map.AddMapDetailToMap(detail);
        }
    }
}