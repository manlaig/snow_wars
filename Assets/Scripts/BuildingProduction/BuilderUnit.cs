using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
public class BuilderUnit : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject currentBuildingToBuild;
    Animation animation;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animation>();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventManager.Events.NewBuildingPlaced, NewBuildingPlaced);
        EventManager.StartListening(EventManager.Events.WorkerFinishedBuilding, FinishedBuilding);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.NewBuildingPlaced, NewBuildingPlaced);
        EventManager.StopListening(EventManager.Events.WorkerFinishedBuilding, FinishedBuilding);
    }

    void NewBuildingPlaced(GameObject building)
    {
        currentBuildingToBuild = building;
        agent.SetDestination(building.transform.position);
        if(animation)
            animation.Play(animation.clip.name.Split('|')[0] + "|Run");
    }

    void FinishedBuilding(GameObject go)
    {
        Debug.Log("Finished building");
        agent.isStopped = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!currentBuildingToBuild)
            return;
        if (Vector3.Distance(transform.position, currentBuildingToBuild.transform.position) < 15)
        {
            ShowBuildProgress progress = currentBuildingToBuild.GetComponent<ShowBuildProgress>();
            if (progress)
            {
                Debug.Log("Reached building to build");
                progress.StartBuilding();

                ResetAgent();
                agent.isStopped = true;
            }
            currentBuildingToBuild = null;
        }
    }

    void ResetAgent()
    {
        agent.ResetPath();
        if (animation)
            animation.Play(animation.clip.name.Split('|')[0] + "|Idle");
    }
}
