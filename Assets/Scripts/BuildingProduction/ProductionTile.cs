using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// required for collision detection
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ProductionTile : MonoBehaviour
{
    [SerializeField] Material tileMaterial;
    [SerializeField] LayerMask collisionLayers;

    public bool colliding { get; private set; } 

	void Start ()
    {
        SetColor(Color.green);
    }

    void SetColor(Color c)
    {
        tileMaterial.SetColor("_TintColor", c);
    }

    void OnTriggerEnter(Collider other)
    {
        if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            SetColor(Color.red);
            colliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            SetColor(Color.green);
            colliding = false;
        }
    }
}
