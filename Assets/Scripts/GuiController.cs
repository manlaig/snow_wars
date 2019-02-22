using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// GUI controller.
/// </summary>
public class GuiController : MonoBehaviour
{

    private int activeSquad = 1;
    [SerializeField]
    private SquadManager squadManager = null;
    private List<GameObject> selectedObjects;
    [SerializeField]
    private GameObject unitTray = null;
    [SerializeField]
    private GameObject unitPrefab = null;
    [SerializeField]
    private Texture2D cursor = null;
    [SerializeField]
    private Texture2D attackCursor = null;

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        if (squadManager)
            EventManager.StartListening(EventManager.Events.UnitKilled, RemoveFromAllSquads);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        if (squadManager)
            EventManager.StopListening(EventManager.Events.UnitKilled, RemoveFromAllSquads);
    }

    void Start()
    {
        selectedObjects = new List<GameObject>();
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Sets the selected objects.
    /// </summary>
    /// <param name="objects">Objects.</param>
    public void SetSelectedObjects(List<GameObject> objects)
    {
        selectedObjects = objects;
    }

    /// <summary>
    /// Sets the active squad.
    /// </summary>
    /// <param name="squad">Squad.</param>
    public void SetActiveSquad(int squad)
    {
        if (squad <= SquadManager.NUMBER_OF_SQUADS &&
            squad > 0)
        {
            activeSquad = squad;
        }

        UpdateSquadList();
    }

    /// <summary>
    /// Gets the active squad.
    /// </summary>
    /// <returns>The active squad.</returns>
    public int GetActiveSquad()
    {
        return activeSquad;
    }

    /// <summary>
    /// Deletes the active squad.
    /// </summary>
    public void DeleteActiveSquad()
    {
        if(squadManager)
            squadManager.DeleteSquad(activeSquad);
        Debug.Log("Removed squad " + activeSquad + ".");
        UpdateSquadList();
    }

    /// <summary>
    /// Adds the selected to active squad.
    /// </summary>
    public void AddSelectedToActiveSquad()
    {
        squadManager.AddToSquad(activeSquad, selectedObjects);
        EventManager.TriggerEvent(EventManager.Events.ClearSelection, gameObject);
        UpdateSquadList();
    }

    /// <summary>
    /// Removes the selected from active squad.
    /// </summary>
    public void RemoveSelectedFromActiveSquad()
    {
        squadManager.RemoveFromSquad(activeSquad, selectedObjects);
        UpdateSquadList();
    }

    /// <summary>
    /// Removes unit from all squads.
    /// </summary>
    /// <param name="go">Unit GameObject</param>
    public void RemoveFromAllSquads(GameObject go)
    {
        if (go.transform.IsChildOf(transform.root))
        {
            squadManager.RemoveFromAllSquads(go);
            UpdateSquadList();
        }
    }

    /// <summary>
    /// Squad or Unit do action.
    /// </summary>
    /// <param name="action">Action.</param>
    public void SquadOrUnitAction(UnitCommands.Actions action, Transform target = null, Vector3 destination = default(Vector3))
    {
        if (selectedObjects.Count > 0)
        {
            UnitCommands.UnitAction(selectedObjects, action);
        }
        else
        {
            squadManager.SquadAction(activeSquad, action);
        }
    }


    IEnumerator WaitForCommand(UnitCommands.Actions action)
    {

        RaycastHit hit;
        Ray ray;

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);

        while (true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                }
                else if (hit.transform.GetComponent<WorldObject>() && hit.transform.root != transform.root)
                {
                    Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
                }
                //else
                //    Debug.Log(hit);

                if (Input.GetMouseButtonDown(1))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        SquadOrUnitAction(action, null, hit.transform.position);
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        yield break;
                    }
                    else if (hit.transform.GetComponent<WorldObject>() && hit.transform.root != transform.root)
                    {
                        SquadOrUnitAction(action, hit.transform);
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                        yield break;
                    }
                    else
                    {
                        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    yield break;
                }

            }


            yield return null;
        }
    }


    /// <summary>
    /// Do the attack (button).
    /// </summary>
    public void DoAttack()
    {
        StartCoroutine(WaitForCommand(UnitCommands.Actions.Attack));
    }

    /// <summary>
    /// Do the hault (button).
    /// </summary>
    public void DoHault()
    {
        SquadOrUnitAction(UnitCommands.Actions.Halt);
    }

    /// <summary>
    /// Do the micro ability (button).
    /// </summary>
    public void DoMicroAbility()
    {
        SquadOrUnitAction(UnitCommands.Actions.MicroAbility);
    }

    /// <summary>
    /// Do the patrol (button).
    /// </summary>
    public void DoPatrol()
    {
        SquadOrUnitAction(UnitCommands.Actions.Patrol);
    }

    private List<Unit> GetCurrentSquadUnits()
    {
        return squadManager.GetUnitsFromSquad(activeSquad).Select(go => go.GetComponent<Unit>()).ToList();
    }

    private List<Unit> GetSortedCurrentSquadUnits()
    {
        return GetCurrentSquadUnits().OrderByDescending(bw => bw.PRIORITY).ToList();
    }

    public void UpdateSquadList()
    {
        List<Unit> unitList = GetSortedCurrentSquadUnits();
        GameObject unitTemp;
        RectTransform unitTempRT;
        RectTransform unitPrefabRT = unitPrefab.GetComponent<RectTransform>();

        foreach (Transform item in unitTray.transform)
        {
            Destroy(item.gameObject);
        }

        int i = 0;

        foreach (var unit in unitList)
        {
            Debug.Log("Adding UnitTrayItem for " + unit.name);
            unitTemp = Instantiate(unitPrefab, unitTray.transform, false);
            unitTempRT = unitTemp.GetComponent<RectTransform>();
            unitTempRT.SetParent(unitTray.GetComponent<RectTransform>());

            unitTempRT.localPosition =
                new Vector3(
                    unitPrefabRT.rect.width * i++,
                    unitTempRT.localPosition.y,
                    unitTempRT.localPosition.z
                );

            unitTemp.GetComponent<UnitTrayItem>().SetupUnitLink(unit);
        }
    }
}
