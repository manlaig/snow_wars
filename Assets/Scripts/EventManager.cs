using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Event manager.
/// </summary>
public class EventManager : MonoBehaviour
{
    private Dictionary<Events, List<UnityAction<GameObject>>> eventDictionary;

    public enum Events
    {
        TreeDead,
        CastleDistroyed,
        HealthUpdate,
        UnitKilled,
        SnowballUpdate,
        SupplyUpdate,
        ClearSelection,
        NewBuildingPlaced,
        WorkerFinishedBuilding
    }

    private static EventManager eventManager;

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    /// <summary>
    /// Init this instance.
    /// </summary>
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<Events, List<UnityAction<GameObject>>>();
        }
    }

    /// <summary>
    /// Starts the listening.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="listener">Listener.</param>
    public static void StartListening(Events eventName, UnityAction<GameObject> listener)
    {
        List<UnityAction<GameObject>> thisEvent;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Add(listener);
        }
        else
        {
            thisEvent = new List<UnityAction<GameObject>>() { };
            thisEvent.Add(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    /// Stops the listening.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="listener">Listener.</param>
    public static void StopListening(Events eventName, UnityAction<GameObject> listener)
    {
        if (eventManager == null) return;

        List<UnityAction<GameObject>> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Remove(listener);
        }
    }

    /// <summary>
    /// Triggers the event.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    public static void TriggerEvent(Events eventName, GameObject callback = null)
    {
        List<UnityAction<GameObject>> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            foreach (var action in thisEvent)
            {
                action.Invoke(callback);
            }
        }
    }
}
