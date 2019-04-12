using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Snowball : MonoBehaviour
{
    [SerializeField] GameObject explode_effect;
    [SerializeField] float speed = 3f;

    [HideInInspector] public Transform target;
    // the ControlRange script specifies the damage to deal on the target
    [HideInInspector] public float damage = 10f;

    void Start()
    {
        // the snowball will initially play the spinning animation
        GetComponent<Animation>().CrossFade("Snowball|Spinning");
    }

    void OnTriggerEnter(Collider other)
    {
        // preventing nullReferenceException
        if (target && other.gameObject == target.gameObject)
        {
            // destroy the snowball explode affect after playing
            Destroy(Instantiate(explode_effect, transform.position, transform.rotation), 1f);
            target.gameObject.GetComponent<ControlBasic>().GetHit(damage, gameObject);
            Destroy(gameObject);
        }
    }

    void Update ()
    {
		if(target != null)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed / 10f);
	}
}
