using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Workers Class (Collects and Uses Snowballs).
/// </summary>
public class Workers : MonoBehaviour
{
    List<BuilderUnit> buildersInScene;
    List<BuilderUnit> idleBuilders;
    
    // mapping between which builder is building which building
    Dictionary<GameObject, BuilderUnit> builderMap;

    private static bool isApplicationQuitting = false;
    private static Workers _instance;
    public static Workers instance
    {
        get
        {
            if (_instance == null && !isApplicationQuitting)
            {
                _instance = new GameObject("Worker Manager").AddComponent<Workers>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    void OnDestroy()
    {
        /* the scene is being destroyed, since Unity destroys GameObjects in random order,
         * this instance can get destroyed before the builders in the scene.
         * If that happens, an error will be thrown, so we'll prevent it by checking with this variable
         */
        isApplicationQuitting = true;
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

    Workers()
    {
        isApplicationQuitting = false;
        idleBuilders = new List<BuilderUnit>();
        buildersInScene = new List<BuilderUnit>();
        builderMap = new Dictionary<GameObject, BuilderUnit>();
    }

    public void addWorker(BuilderUnit builder)
    {
        idleBuilders.Add(builder);
        buildersInScene.Add(builder);
    }

    public void removeWorker(BuilderUnit builder)
    {
        buildersInScene.Remove(builder);
        if(idleBuilders.Contains(builder))
            idleBuilders.Remove(builder);
    }

    public int getWorkersCount()
    {
        return buildersInScene.Count;
    }

    void NewBuildingPlaced(GameObject building)
    {
        List<BuilderUnit> listToIterate = buildersInScene;
        if (idleBuilders.Count > 0)
            listToIterate = idleBuilders;

        float closestDistance = float.MaxValue;
        BuilderUnit closestBuilder = listToIterate[0];
        foreach(BuilderUnit unit in listToIterate)
        {
            Vector3 pos = unit.gameObject.transform.position;
            float distance = Vector3.Distance(pos, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBuilder = unit;
            }
        }

        closestBuilder.AddBuildingInQueue(building);
        builderMap[building] = closestBuilder;
        if (listToIterate == idleBuilders)
        {
            idleBuilders.Remove(closestBuilder);
        }
    }

    void FinishedBuilding(GameObject go)
    {
        if (builderMap.ContainsKey(go))
        {
            builderMap[go].FinishBuilding();
            idleBuilders.Add(builderMap[go]);
        }
    }
}
