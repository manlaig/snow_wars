using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildingSystem;

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

    #region Instance Objects
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
            /*
             * Since currentSpawnedBuilding != null, there's already a building chosen to build
             * place the building on left mouse click
             */
            if (Input.GetMouseButtonDown(0))
            {
                // don't build if mouse if click is not on terrainLayer
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
            if(PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                currentSpawnedBuilding.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
    }


    bool CanPlaceBuilding()
    {
        if (PlacementHelpers.IsButtonPressed(uiRaycaster))
        {
            return false;
        }
        for (int i = 0; i < activeTiles.Count; i++)
            if (activeTiles[i].colliding)
                return false;
        return true;
    }


    void PlaceBuilding()
    {
        ClearGrid();
        StartCoroutine(BeginBuilding());
    }


    void ClearGrid()
    {
        Destroy(activeTilesParent);
        activeTiles.Clear();
    }


    IEnumerator BeginBuilding()
    {
        Vector3 pos = currentSpawnedBuilding.transform.position;
        GameObject instance = currentSpawnedBuilding;
        currentSpawnedBuilding = null;

        RaycastHit hitTerrain;
        if (PlacementHelpers.RaycastFromMouse(out hitTerrain, terrainLayer))
            pos = hitTerrain.point;

        GameObject underConstructionIns = Instantiate(underConstructionGO, pos, Quaternion.identity);
        EventManager.TriggerEvent(EventManager.Events.NewBuildingPlaced, underConstructionIns);

        // wait until the worker reaches the building
        while (!underConstructionIns.GetComponent<ShowBuildProgress>().started)
            yield return null;

        yield return new WaitForSeconds(buildingToPlace.currentBuilding.buildTime);
        Debug.Log("waited " + buildingToPlace.currentBuilding.buildTime + " seconds to build " + buildingToPlace.currentBuilding.name);

        // activating the mesh renderers
        PlacementHelpers.ToggleRenderers(instance, true);

        /*
         * Buildings need to be nav mesh obstacles, so that the worker and other agents avoid it while finding path
         * When a building is finished building, it needs to become an obstacle for agents
         */
        PlacementHelpers.ToggleNavMeshObstacle(instance, true);

        Destroy(underConstructionIns);

        EventManager.TriggerEvent(EventManager.Events.WorkerFinishedBuilding, null);
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

        GameObject parent = new GameObject("PlacementGrid");
        parent.transform.SetParent(col.gameObject.transform.root);
        parent.transform.position = col.gameObject.transform.position;

        activeTiles.Clear();
        for (float i = fromX; i < toX; i += productionTile.transform.localScale.x)
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

    
    // make a BuildingSO object for all buildings that can be placed, and call this function
    public void SpawnBuilding(BuildingSO building)
    {
        // if haven't placed the spawned building, then return
        if (currentSpawnedBuilding)
            return;

        currentSpawnedBuilding = Instantiate(building.buildingPrefab);
        buildingToPlace.currentBuilding = building;

        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, false);
        PlacementHelpers.ToggleNavMeshObstacle(currentSpawnedBuilding, false);

        Collider[] cols = currentSpawnedBuilding.GetComponentsInChildren<Collider>();
        if(cols.Length > 0)
            FillRectWithTiles(cols[0]);
        else
            Debug.LogError("Building has no colliders");
    }
}
