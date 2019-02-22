/// <summary>
/// BuildingProduction.
///
/// Purpose:	Allows units to be produced from buildings.
/// Use:		Attach this script to buildings that can produce units
/// Required Setup:
///				Initialize the unit variables in the inspector
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building Production Class
/// </summary>
public class BuildingProduction : MonoBehaviour
{
    /*
    protected RaycastHit hitInfo = new RaycastHit();
    [SerializeField]
    protected bool selected = false;
    public float spawnVariance = 5;

    // Spawn Variables set in Inspector
    public GameObject spawnUnit1;
    public Texture2D unitIcon1;
    protected int cost1;
    public GameObject spawnUnit2;
    public Texture2D unitIcon2;
    protected int cost2;
    // Spawn Location
    public Vector3 spawnLocation;
    public bool spawnChosen = false;
    protected GameObject selectionCircle = null;
    public GameObject prefab_ChooseCircle;

    // Use this for initialization
    void Start()
    {
        cost1 = spawnUnit1.GetComponent<WorldObject>().cost;
        cost2 = spawnUnit2.GetComponent<WorldObject>().cost;
    }

    // Update is called once per frame
    void Update()
    {
        /// Implement Left click to toggle selected when ray hits self
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                // team == 1 for testing purposes, replace with code to check if team is the players team
                if (hitInfo.transform.position == transform.position)
                    selected = true;
                else
                    selected = false;
            }
        }
    }

    public void toggleSelected()
    {
        selected = !(selected);
    }

    void OnGUI()
    {
        if (selected)
        {
            GUI.color = Color.blue;
            GUI.backgroundColor = Color.blue;
            GUI.contentColor = Color.black;

            // Get screen location of building
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            int windowWidth = 200;
            int windowHeight = 210;

            // Create menu window
            Rect menuRect = new Rect(screenPos.x - windowWidth / 2, (Screen.height - screenPos.y) - windowHeight / 2, windowWidth, windowHeight);
            menuRect = GUI.Window(0, menuRect, WindowFunction, "Igloo");

        }
        else if (selectionCircle != null)
        {
            Destroy(selectionCircle.gameObject);
            selectionCircle = null;
        }
    }

    void WindowFunction(int windowID)
    {
        if (spawnChosen && selectionCircle != null)
        {
            Destroy(selectionCircle.gameObject);
            selectionCircle = null;
        }
        if (spawnChosen) // There is a spawnLocation
        {
            // Set locations and dimensions of menu items
            Rect label1 = new Rect(10, 20, 120, 50);
            Rect icon1 = new Rect(140, 20, 50, 50);
            Rect label2 = new Rect(10, 80, 120, 50);
            Rect icon2 = new Rect(140, 80, 50, 50);
            Rect label3 = new Rect(10, 140, 120, 50);
            Rect icon3 = new Rect(140, 140, 50, 50);

            int snowballs = transform.root.GetComponent<Resources>().GetSnowballCount();

            // USS button
            GUI.Label(label1, "USS (cost " + cost1 + " sb)");
            if (GUI.Button(icon1, unitIcon1) && snowballs >= cost1)
            {
                GameObject spawn;
                // Subtract production cost from total
                transform.root.GetComponent<Resources>().RemoveSnowballs(cost1);
                // Spawn unit
                spawn = Instantiate(spawnUnit1, transform.root.transform);
                // Randomize Spawn Position
                spawn.transform.position += new Vector3(spawnVariance * (Random.value - 0.5f), 0, spawnVariance * (Random.value - 0.5f)) + spawnLocation;
            }
            if (snowballs < cost1) // cover button if insufficient snowballs
            {
                GUI.Box(icon1, "");
            }

            // Gingerbread Man button
            GUI.Label(label2, "Gingerbread Man (cost " + cost2 + " sb)");
            if (GUI.Button(icon2, unitIcon2) && snowballs >= cost2)
            {
                GameObject spawn;
                // Subtract production cost from total
                transform.root.GetComponent<Resources>().RemoveSnowballs(cost2);
                // Spawn Unit
                spawn = Instantiate(spawnUnit2, transform.root.transform);
                // Randomize Spawn Position
                spawn.transform.position += new Vector3(spawnVariance * (Random.value - 0.5f), 0, spawnVariance * (Random.value - 0.5f)) + spawnLocation;
            }
            if (snowballs < cost2) // cover button if insufficient snowballs
            {
                GUI.Box(icon2, "");
            }

            // Reset spawnLocation button
            GUI.Label(label3, "Reset Spawn Location");
            if (GUI.Button(icon3, ""))
            {
                spawnChosen = false;
            }
        }
        else // need to choose a spawnLocation
        {
            Rect label2 = new Rect(10, 80, 120, 50);
            GUI.Label(label2, "Right Click to Select Spawn Location");

            if (selectionCircle == null)
            {
                selectionCircle = Instantiate(prefab_ChooseCircle);
                selectionCircle.transform.SetParent(transform, false);
                selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    // check clicked on ground and within range
                    if (hitInfo.transform.name == "Terrain" && (hitInfo.point - transform.position).sqrMagnitude <= (15 * 15))
                    {
                        spawnLocation = (hitInfo.point - transform.root.transform.position);
                        spawnChosen = true;
                        Destroy(selectionCircle.gameObject);
                        selectionCircle = null;
                    }
                }
            }
        }
    }
    */
}
