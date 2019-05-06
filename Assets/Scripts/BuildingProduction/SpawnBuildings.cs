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

    [Tooltip("How high to scan for collision")]
    [SerializeField] float scanHeight = 2f;

    [SerializeField] GameObject underConstructionGO;
    [SerializeField] BuildProgressSO buildingToPlace;
    #endregion

    #region Instance Objects
    GameObject currentSpawnedBuilding;
    List<ProductionTile> activeTiles;
    GameObject activeTilesParent; // the parent object that contains the grid tiles
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
                RaycastHit hit;

                // don't build if mouse if click is not on terrainLayer
                if (!PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
                    return;

                currentSpawnedBuilding.transform.position = hit.point;
                activeTilesParent.transform.position = hit.point;

                if (CanPlaceBuilding())
                    PlaceBuilding();
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    void FixedUpdate()
    {
        if (currentSpawnedBuilding)
        {
            RaycastHit hit;
            if (PlacementHelpers.RaycastFromMouse(out hit, terrainLayer))
            {
                currentSpawnedBuilding.transform.position = hit.point;
                activeTilesParent.transform.position = hit.point;
            }
        }
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
        // by the time we start building, buildingToPlace can be changed
        // so, we need to store the build time ASAP
        float buildTime = buildingToPlace.currentBuilding.buildTime;

        // this will allow us to put build requests before the current one finishes
        GameObject instance = currentSpawnedBuilding;
        currentSpawnedBuilding = null;

        RaycastHit hitTerrain;
        if (!PlacementHelpers.RaycastFromMouse(out hitTerrain, terrainLayer))
            yield break;

        GameObject underConstructionIns = Instantiate(underConstructionGO, hitTerrain.point, Quaternion.identity);

        // worker unit manager listens for this event and it will assign the appropriate worker
        EventManager.TriggerEvent(EventManager.Events.NewBuildingPlaced, underConstructionIns);

        // wait until the worker reaches the building
        while (!underConstructionIns.GetComponent<ShowBuildProgress>().started)
            yield return null;

        yield return new WaitForSeconds(buildTime);
        Debug.Log("waited " + buildingToPlace.currentBuilding.buildTime + " seconds to build " + buildingToPlace.currentBuilding.name);

        // we finished waiting the buildTime at this point, activate the building
        instance.SetActive(true);

        // worker unit manager listens for this event and it will rest the worker that built it 
        EventManager.TriggerEvent(EventManager.Events.WorkerFinishedBuilding, underConstructionIns);
        Destroy(underConstructionIns);
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
        parent.transform.position = col.gameObject.transform.position;

        activeTiles.Clear();
        for (float i = fromX; i < toX; i += productionTile.transform.localScale.x)
        {
            for(float j = fromZ; j < toZ; j += productionTile.transform.localScale.y)
            {
                GameObject tile = Instantiate(productionTile);
                tile.transform.SetParent(parent.transform);
                tile.transform.position = new Vector3(i, parent.transform.position.y + scanHeight, j);
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

        // the underConstruction object accesses this ScriptableObject to show the building progress
        buildingToPlace.currentBuilding = building;

        currentSpawnedBuilding = Instantiate(building.buildingPrefab);

        Collider[] cols = currentSpawnedBuilding.GetComponentsInChildren<Collider>();
        if(cols.Length > 0)
            FillRectWithTiles(cols[0]);
        else
            Debug.LogError("Building has no colliders");

        // make the building invisible in the scene, since it hasn't been placed
        currentSpawnedBuilding.SetActive(false);
    }
}
