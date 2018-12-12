using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SpawnBuildings : MonoBehaviour
{
    #region Inspector Variables
    // TODO: assign these layermasks in a script
    [TooltipAttribute("The tile GameObject that make up the grid")]
    [SerializeField] GameObject productionTile;

    [TooltipAttribute("The layer in which the terrain is placed")]
    [SerializeField] LayerMask terrainLayer;

    [TooltipAttribute("Need GraphicRaycaster to detect click on a button")]
    [SerializeField] GraphicRaycaster uiRaycaster;

    [SerializeField] GameObject underConstructionGO;
    [SerializeField] BuildProgressSO buildingToPlace;
    #endregion

    #region Private Objects
    GameObject currentSpawnedBuilding;
    RaycastHit hit;
    List<ProductionTile> activeTiles;
    GameObject activeTilesParent;
    #endregion

    void Start ()
    {
        activeTiles = new List<ProductionTile>();
        if (!productionTile)
            Debug.LogError("Production Tile is NULL");
        if (!uiRaycaster)
            Debug.Log("GraphicRaycaster not found! Will place objects on button click");
	}


    void Update()
    {
        if (currentSpawnedBuilding)
        {
            if (Input.GetMouseButtonDown(0))
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
        if (PlacementHelpers.IsButtonPressed(uiRaycaster))
            return false;
        for(int i = 0; i < activeTiles.Count; i++)
            if(activeTiles[i].colliding)
                return false;
        return true;
    }


    void PlaceBuilding()
    {
        StartCoroutine(BeginBuilding());
        ClearGrid();
    }


    IEnumerator BeginBuilding()
    {
        Vector3 pos = currentSpawnedBuilding.transform.position;
        GameObject instance = currentSpawnedBuilding;
        currentSpawnedBuilding = null;
        activeTilesParent = null;

        RaycastHit hitTerrain;
        if (PlacementHelpers.RaycastFromMouse(out hitTerrain, terrainLayer))
            pos = hitTerrain.point;

        GameObject go = Instantiate(underConstructionGO, pos, Quaternion.identity);
        yield return new WaitForSeconds(buildingToPlace.currentBuilding.buildTime);
        Debug.Log("waited " + buildingToPlace.currentBuilding.buildTime + " seconds to build " + buildingToPlace.currentBuilding.name);
        PlacementHelpers.ToggleRenderers(instance, true);
        Destroy(go);
    }


    void ClearGrid()
    {
        for(int i = 0; i < activeTiles.Count; i++)
            if(activeTiles[i] != null)
                Destroy(activeTiles[i].gameObject);
        activeTiles.RemoveAll(i => i);
    }


    void FillRectWithTiles(Collider col)
    {
        if (activeTilesParent)
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
                tile.transform.position = new Vector3(i, parent.transform.position.y + 2, j);
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
        buildingToPlace.currentBuilding = building;
        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, false);
        Collider[] cols = currentSpawnedBuilding.GetComponentsInChildren<Collider>();
        if(cols.Length > 0)
            FillRectWithTiles(cols[0]);
        else
            Debug.LogError("Building has no colliders");
    }
}
