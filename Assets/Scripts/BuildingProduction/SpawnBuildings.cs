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
    List<ProductionTile> activeTiles;


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
                PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, true);
                currentSpawnedBuilding = null;
                activeTiles.RemoveAll(i => i); // not sure about this line

                /*Rect r = PlacementHelpers.MakeRectOfCollider(currentSpawnedBuilding.GetComponentsInChildren<Collider>()[0]);
                FillRectWithTiles(currentSpawnedBuilding.GetComponentsInChildren<Collider>()[0]);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube.transform.position = new Vector3(r.position.x, 30, r.position.y);
                GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube2.transform.position = new Vector3(r.position.x + r.width, 30, r.position.y + r.height);
                GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube3.transform.position = new Vector3(r.position.x, 30, r.position.y + r.height);
                GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube4.transform.position = new Vector3(r.position.x + r.width, 30, r.position.y);*/
            }
            if (Input.GetMouseButtonDown(1))
                Destroy(currentSpawnedBuilding);
        }
    }


    void FillRectWithTiles(Collider col)
    {
        Rect rect = PlacementHelpers.MakeRectOfCollider(col);
        float fromX = rect.position.x;
        float toX = rect.position.x + rect.width;
        float fromZ = rect.position.y;
        float toZ = rect.position.y + rect.height;

        GameObject parent = new GameObject("ProductionTileParent");
        parent.transform.SetParent(col.gameObject.transform);
        parent.transform.position = col.gameObject.transform.position;

        for(float i = fromX; i < toX; i += productionTile.transform.localScale.x)
        {
            for(float j = fromZ; j < toZ; j += productionTile.transform.localScale.y)
            {
                GameObject tile1 = Instantiate(productionTile);
                tile1.transform.SetParent(parent.transform);
                tile1.transform.position = new Vector3(i, parent.transform.position.y, j);
            }
        }

    }


    public void SpawnBuilding(BuildingSO building)
    {
        // if haven't placed the spawned building, then return
        if (currentSpawnedBuilding)
            return;
        currentSpawnedBuilding = Instantiate(building.buildingPrefab);
        PlacementHelpers.ToggleRenderers(currentSpawnedBuilding, false);
    }
}
