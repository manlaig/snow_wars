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

    GameObject currentSpawnedBuilding;
    RaycastHit hit;
    List<ProductionTile> activeTiles;
    GameObject activeTilesParent;


	void Start ()
    {
        activeTiles = new List<ProductionTile>();
        if (!productionTile)
            Debug.LogError("Production Tile is NULL");
	}


    void Update()
    {
        if (currentSpawnedBuilding)
        {
            if (Input.GetMouseButtonDown(0) /*&& !RaycastFromMouse(out uiHit, uiLayer)*/)
            {
                if (!PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                    return;

                currentSpawnedBuilding.transform.position = hit.point;

                if(CanPlaceBuilding())
                    PlaceBuilding();
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    void FixedUpdate()
    {
        if(currentSpawnedBuilding)
        {
            if(PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                currentSpawnedBuilding.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
        }
    }


    bool CanPlaceBuilding()
    {
        for(int i = 0; i < activeTiles.Count; i++)
            if(activeTiles[i].colliding)
                return false;
        return true;
    }


    void PlaceBuilding()
    {
        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, true);
        currentSpawnedBuilding = null;
        activeTilesParent = null;
        ClearList();
    }


    void ClearList()
    {
        for(int i = 0; i < activeTiles.Count; i++)
        {
            if(activeTiles[i] != null)
                Destroy(activeTiles[i].gameObject);
        }
        activeTiles.RemoveAll(i => i);
    }


    void FillRectWithTiles(Collider col)
    {
        if(activeTilesParent)
            return;

        Rect rect = PlacementHelpers.MakeRectOfCollider(col);
        float fromX = rect.position.x;
        float toX = rect.position.x + rect.width;
        float fromZ = rect.position.y;
        float toZ = rect.position.y + rect.height;

        GameObject parent = new GameObject("ProductionTileParent");
        parent.transform.SetParent(col.gameObject.transform.root.gameObject.transform);
        parent.transform.position = col.gameObject.transform.position;

        for(float i = fromX; i < toX; i += productionTile.transform.localScale.x)
        {
            for(float j = fromZ; j < toZ; j += productionTile.transform.localScale.y)
            {
                GameObject tile = Instantiate(productionTile);
                tile.transform.SetParent(parent.transform);
                tile.transform.position = new Vector3(i, parent.transform.position.y + 1, j);
                activeTiles.Add(tile.GetComponent<ProductionTile>());
            }
        }
        activeTilesParent = parent;
    }


    public void SpawnBuilding(BuildingSO building)
    {
        // if haven't placed the spawned building, then return
        if (currentSpawnedBuilding)
            return;
        currentSpawnedBuilding = Instantiate(building.buildingPrefab);
        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, false);
        Collider[] cols = currentSpawnedBuilding.GetComponentsInChildren<Collider>();
        if(cols.Length > 0)
            FillRectWithTiles(cols[0]);
        else
            Debug.LogError("Building has no colliders");
    }
}
