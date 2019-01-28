﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
public class BuilderUnit : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject currentBuildingToBuild;
    Animation animation;
    Queue<GameObject> buildingsQ;
    bool idle;

    void Start()
    {
        idle = true;
        buildingsQ = new Queue<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animation>();
    }

    void Update()
    {
        if(buildingsQ.Count > 0 && idle)
        {
            WorkOnBuilding(buildingsQ.Dequeue());
        }
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
        /*
         * Later on, when we'll have multiple workers, use a different class 
         * to find the closest worker and enqueue the building to it
         */
        buildingsQ.Enqueue(building);
    }

    void WorkOnBuilding(GameObject go)
    {
        currentBuildingToBuild = go;
        agent.SetDestination(go.transform.position);
        idle = false;
        PlayAnimation("Run");

    }

    void FinishedBuilding(GameObject go)
    {
        agent.isStopped = false;
        agent.ResetPath();
        PlayAnimation("Idle");

        idle = true;
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
                progress.StartBuilding();

                agent.isStopped = true;
                PlayAnimation("Gather");

                currentBuildingToBuild = null;
            }
        }
    }

    void PlayAnimation(string name)
    {
        if (animation)
            animation.Play(animation.clip.name.Split('|')[0] + "|" + name);
    }
}
