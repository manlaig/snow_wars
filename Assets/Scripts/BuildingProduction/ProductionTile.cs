using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProductionTile : MonoBehaviour
{
    [SerializeField] Material tileMaterial;
	void Start ()
    {
        //tileMaterial.color = Color.green;
        tileMaterial.SetColor("_TintColor", Color.green);
    }
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Environment"))
            tileMaterial.color = Color.red;
        //else
            //GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
