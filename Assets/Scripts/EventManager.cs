using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Event manager.
/// Very common events like Left and Right mouse clicks are triggered from this script
/// </summary>
public class EventManager : MonoBehaviour
{
    // we want to display GUI when a mouse clicked on a building,
    // each building listening for mouse events is not efficient,
    // we'll let this class listen for mouse events and notify buildings, units, etc.
    [Tooltip("All layers that will need to listen for mouse clicked and moved events")]
    [SerializeField] LayerMask mouseListenerLayer;
    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D attackCursor;

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
        WorkerFinishedBuilding,
        LeftMouseClickedDown,
        RightMouseClickedDown,
        LeftMouseClickedUp,
        RightMouseClickedUp,
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



    ///////////////// Triggering common events /////////////////
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TriggerEvent(Events.LeftMouseClickedDown);
            HandleMouseClick();
        }
        else if (Input.GetMouseButtonUp(0))
            TriggerEvent(Events.LeftMouseClickedUp);
        if (Input.GetMouseButtonDown(1))
            TriggerEvent(Events.RightMouseClickedDown);
        else if (Input.GetMouseButtonUp(1))
            TriggerEvent(Events.RightMouseClickedUp);

        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mouseListenerLayer))
            {
                HandleMouseHover(hit);
            }
        }
    }

    void HandleMouseClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mouseListenerLayer))
        {
            if (hit.transform.gameObject.GetComponent<BaseBuilding>() && hit.transform.gameObject.activeInHierarchy)
            {
                // the mouse clicked on a building
                hit.transform.gameObject.GetComponent<BaseBuilding>().OnClick();
            }
        }
    }

    void HandleMouseHover(RaycastHit hit)
    {
        if (!hit.transform.gameObject.activeInHierarchy)
            return;
        if (hit.transform.gameObject.GetComponent<BaseBuilding>())
        {
            // the mouse is hovering over a building
            hit.transform.gameObject.GetComponent<BaseBuilding>().OnMouseHover();
        }
        else if (hit.transform.gameObject.GetComponent<Unit>() && !hit.transform.root.transform.gameObject.GetComponent<Player>())
        {
            // the mouse is hovering over an enemy troop
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
