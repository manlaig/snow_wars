using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
public class BuilderUnit : MonoBehaviour
{
    [Tooltip("The distance at which the builder can start build")]
    [SerializeField] float buildingDistance = 25f;

    NavMeshAgent agent;
    GameObject currentBuildingToBuild;
    Animation animation;
    Queue<GameObject> buildingsQ;

    public bool idle { get; private set; }

    void Start()
    {
        idle = true;
        buildingsQ = new Queue<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animation>();

        Workers w = Workers.instance;
        w.addWorker(this);
    }

    void OnDestroy()
    {
        if(Workers.instance)
            Workers.instance.removeWorker(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision();
    }

    void OnTriggerEnter(Collider other)
    {
        HandleCollision();
    }

    void Update()
    {
        if(buildingsQ.Count > 0 && idle)
            StartBuilding();

        // the worker can build if he gets close enough to the building
        if (currentBuildingToBuild)
            if (Vector3.Distance(transform.position, currentBuildingToBuild.transform.position) <= buildingDistance)
            {
                HandleCollision();
            }
    }

    void HandleCollision()
    {
        if (!currentBuildingToBuild)
            return;
        if (Vector3.Distance(transform.position, currentBuildingToBuild.transform.position) <= buildingDistance)
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

    void StartBuilding()
    {
        GameObject nextInQueue = buildingsQ.Dequeue();
        currentBuildingToBuild = nextInQueue;
        agent.SetDestination(nextInQueue.transform.position);
        idle = false;
        PlayAnimation("Run");
    }

    void PlayAnimation(string name)
    {
        if (animation)
            animation.Play(animation.clip.name.Split('|')[0] + "|" + name);
    }

    public void AddBuildingInQueue(GameObject building)
    {
        buildingsQ.Enqueue(building);
    }

    public void FinishBuilding()
    {
        agent.isStopped = false;
        agent.ResetPath();
        PlayAnimation("Idle");
        idle = true;
    }
}
