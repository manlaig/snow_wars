using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentTurnInstantly : MonoBehaviour
{
    // IGNORE THIS CODE FOR NOW
    // IT DOESN'T DO WHAT IT'S SUPPOSED TO DO AND IT WILL BE FIXED SOON

    /*
    [SerializeField] float rotationSpeed = 20f;
    private NavMeshAgent agent;
    RaycastHit hitInfo;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, Mathf.Infinity) && (hitInfo.transform != transform))
            {
                agent.destination = hitInfo.point;
                Vector3 destination = agent.destination;

                //if (agent.hasPath)
                    //agent.acceleration = (agent.remainingDistance < 2) ? 80 : 10;

                if ((destination - transform.position).magnitude < 0.1f) return;
                Vector3 direction = (destination - transform.position).normalized;
                Quaternion qDir = Quaternion.LookRotation(direction);
                transform.rotation = qDir; // lerp in a coroutine later
                agent.velocity = direction;
            }
        }
    }
    */
}
