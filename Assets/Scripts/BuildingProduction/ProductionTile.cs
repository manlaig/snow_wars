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
        colliding = false;
        GetComponent<Renderer>().material.CopyPropertiesFromMaterial(tileMaterial);
        SetColor(Color.green);
    }

    void SetColor(Color c)
    {
        GetComponent<Renderer>().material.SetColor("_TintColor", c);
    }

    void OnTriggerEnter(Collider other)
    {
        if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            if(other.gameObject.transform.root.gameObject.GetInstanceID() != transform.root.gameObject.GetInstanceID())
            {
                SetColor(Color.red);
                colliding = true;
            }
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
