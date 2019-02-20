using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace BuildingSystem
{
    // helper methods for building system
    static class PlacementHelpers
    {
        public static bool RaycastFromMouse(out RaycastHit h, LayerMask layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out h, Mathf.Infinity, layer))
                return true;
            return false;
        }


        /*
         * Because some gameobjects have the rig and the mesh separately, some components are attached in child objects
         * That's why we need to get those components from the child using GetComponentInChildren
         */
        public static void ToggleRenderers(GameObject go, bool toggle)
        {
            if (!go)
                return;
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
                foreach (Renderer r in renderers)
                    r.enabled = toggle;
        }

        /*
         * Because some gameobjects have the rig and the mesh separately, some components are attached in child objects
         * That's why we need to get those components from the child using GetComponentInChildren
         */
        public static void ToggleNavMeshObstacle(GameObject go, bool toggle)
        {
            if (!go)
                return;
            NavMeshObstacle[] obstacle = go.GetComponentsInChildren<NavMeshObstacle>();
            if (obstacle.Length > 0)
                foreach (NavMeshObstacle o in obstacle)
                    o.enabled = toggle;
        }


        public static Rect MakeRectOfCollider(Collider col)
        {
            Rect r = new Rect(col.bounds.center.x - col.bounds.extents.x,
                            col.bounds.center.z - col.bounds.extents.z,
                            col.bounds.size.x, col.bounds.size.z);
            return r;
        }


        public static bool IsButtonPressed(GraphicRaycaster raycaster)
        {
            if (!EventSystem.current)
            {
                Debug.LogError("EventSystem not found");
                return true;
            }
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();
            eventData.position = Input.mousePosition;
            raycaster.Raycast(eventData, results);
            return results.Count != 0;
        }
    }
}