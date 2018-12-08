using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBuildings : MonoBehaviour
{
    // TODO: assign these layermasks in a script
    [SerializeField] GameObject productionTile;
    [SerializeField] LayerMask terrainLayer;
    [SerializeField] LayerMask uiLayer; // TODO: down spawn if pressed on UI layer
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
        if (currentSpawnedBuilding && RaycastFromMouse(out hit, terrainLayer))
        {
            currentSpawnedBuilding.transform.position = hit.point;
            //RaycastHit uiHit;
            if (Input.GetMouseButtonDown(0) /*&& !isOverlappingColliders(currentSpawnedBuilding) && !RaycastFromMouse(out uiHit, uiLayer)*/)
            {
                //ToggleRenderers(currentSpawnedBuilding, true);
                currentSpawnedBuilding = null;
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    bool RaycastFromMouse(out RaycastHit hit, LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, layer))
        {
            Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        Debug.Log("Raycast false");
        return false;
    }


    bool isOverlappingColliders(GameObject go)
    {
        Collider[] allCol = go.GetComponentsInChildren<Collider>();
        if (allCol.Length == 0)
        {
            Debug.LogError("Building doesn't have colliders");
            return false;
        }

        Bounds colliderBonds = allCol[0].bounds;
        Collider[] overlaps = Physics.OverlapBox(colliderBonds.center, colliderBonds.extents, Quaternion.identity, environmentLayer);
        if (Array.IndexOf(overlaps, allCol[0]) != -1)
            return overlaps.Length > 1;
        Debug.Log(overlaps.Length);
        return overlaps.Length > 0;
    }


    public void SpawnBuilding(BuildingSO building)
    {
        if (currentSpawnedBuilding)
            return;
        Debug.Log("spawn called");
        currentSpawnedBuilding = Instantiate(building.buidlingPrefab);
        //ToggleRenderers(currentSpawnedBuilding, false);
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
