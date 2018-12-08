using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBuildings : MonoBehaviour
{
    // TODO: assign these layermasks in a script
    [SerializeField] GameObject productionTile;
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] LayerMask uiLayer; // TODO: don't spawn if pressed on UI layer
    [SerializeField] LayerMask environmentLayer;

    GameObject currentSpawnedBuilding;
    RaycastHit hit;


	void Start ()
    {
        if (!productionTile)
            Debug.LogError("Production Tile is NULL");
	}


    void Update()
    {
        if (currentSpawnedBuilding)
        {
            if (Input.GetMouseButtonDown(0) /*&& !isOverlappingColliders(currentSpawnedBuilding) && !RaycastFromMouse(out uiHit, uiLayer)*/)
            {
                if (!RaycastFromMouse(out hit, terrainLayer))
                    return;
                currentSpawnedBuilding.transform.position = hit.point;
                ToggleRenderers(currentSpawnedBuilding, true);
                currentSpawnedBuilding = null;
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    bool RaycastFromMouse(out RaycastHit h, LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out h, layer))
        {
            Debug.Log(h.distance);
            return true;
        }
        Debug.Log("Raycast false");
        return false;
    }


    // TODO: get rid of this function
    // TODO: detect collisions with production tiles instead of overlapboxes
    bool isOverlappingColliders(GameObject go)
    {
        Collider[] allCol = go.GetComponentsInChildren<Collider>();
        if (allCol.Length == 0)
        {
            Debug.LogError("Building doesn't have colliders");
            return false;
        }

        Bounds colliderBonds = allCol[0].bounds;
        // here is the issue, research more into OverlapBox()
        Collider[] overlaps = Physics.OverlapBox(colliderBonds.center, colliderBonds.extents, Quaternion.identity, environmentLayer);
        if (Array.IndexOf(overlaps, allCol[0]) != -1) // here is another issue
            return overlaps.Length > 1;
        Debug.Log(overlaps[0].gameObject.transform.position);
        return overlaps.Length > 0;
    }


    public void SpawnBuilding(BuildingSO building)
    {
        if (currentSpawnedBuilding)
            return;
        Debug.Log("spawn called");
        currentSpawnedBuilding = Instantiate(building.buildingPrefab);
        ToggleRenderers(currentSpawnedBuilding, false);
    }


    void ToggleRenderers(GameObject go, bool toggle)
    {
        if (!go)
            return;
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach(Renderer r in renderers)
                r.enabled = toggle;
        }
    }
}
