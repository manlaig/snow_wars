using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuildings : MonoBehaviour
{
    [SerializeField] GameObject productionTile;

	void Start ()
    {
        if (!productionTile)
            Debug.LogError("Production Tile is NULL");
	}
	
	void Update ()
    {
		
	}
}
