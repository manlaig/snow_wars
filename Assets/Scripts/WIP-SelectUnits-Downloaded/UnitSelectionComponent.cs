using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class UnitSelectionComponent : MonoBehaviour
{
    bool isSelecting = false;
    //bool singleSelection = true;
    Vector3 initialMousePos;

    public GameObject selectionCirclePrefab;
    public GameObject guiController;
    private InGameMenuToggle menuInGame;


    private void OnEnable()
    {
        EventManager.StartListening(EventManager.Events.ClearSelection, ClearSelection);
        EventManager.StartListening(EventManager.Events.LeftMouseClickedDown, StartSelecting);
        EventManager.StartListening(EventManager.Events.LeftMouseClickedUp, StopSelecting);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.ClearSelection, ClearSelection);
        EventManager.StopListening(EventManager.Events.LeftMouseClickedDown, StartSelecting);
        EventManager.StopListening(EventManager.Events.LeftMouseClickedUp, StopSelecting);
    }

    void ClearSelection(GameObject callback)
    {
        Debug.Log("ClearSelection called");
        foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
        {
            if (selectableObject.selectionCircle != null && guiController.transform.root == callback.transform.root)
            {
                Destroy(selectableObject.selectionCircle.gameObject);
                selectableObject.selectionCircle = null;
                selectableObject.GetComponent<ControlBasic>().selected = false;
            }
        }
    }

    void StartSelecting(GameObject not_used)
    {
        Debug.Log("Starting selection");
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (!isMenuActive() && !EventSystem.current.IsPointerOverGameObject())
        {
            isSelecting = true;

            initialMousePos = Input.mousePosition;

            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (selectableObject.selectionCircle != null)
                {
                    Destroy(selectableObject.selectionCircle.gameObject);
                    selectableObject.selectionCircle = null;
                    selectableObject.GetComponent<ControlBasic>().selected = false;
                }
            }
        }
    }

    void StopSelecting(GameObject not_used)
    {
        Debug.Log("Stopping selection");
        // If we let go of the left mouse button, end selection
        if (!isMenuActive())
        {
            // TODO: this block is being executed every time left mouse is pressed
            // FindObjectsOfType is very slow, this block is running it every left mouse press

            List<GameObject> selectedItems = new List<GameObject>();
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                // TODO: Find better way for excluding non-player
                Player player = selectableObject.gameObject.transform.root.GetComponent<Player>();

                if (IsWithinSelectionBounds(selectableObject.gameObject) && player.human)
                {
                    selectableObject.GetComponent<ControlBasic>().selected = true;
                    selectedItems.Add(selectableObject.gameObject);
                }
            }

            if (selectedItems.Count > 0)
            {
                Debug.Log("Selecting " + selectedItems.Count + " units");
            }

            guiController.GetComponent<GuiController>().SetSelectedObjects(selectedItems);
            isSelecting = false;
        }
    }

    void Awake()
    {
        menuInGame = GameObject.Find("Menu-Ingame").GetComponent<InGameMenuToggle>();
    }

    bool isMenuActive()
    {
        return menuInGame.IsMenuActive();
    }

    void Update()
    {
        // Highlight all objects within the selection box
        if( isSelecting )
        {
            // FindObjectsOfType is very expensive, we need a better way
            // Since selecting is called so often, we can't use this method all the time
            // One solution: listen for a event and have a list that will update itself based on active units on the scene
            foreach( var selectableObject in FindObjectsOfType<SelectableUnitComponent>() )
            {
                Player player = selectableObject.gameObject.transform.root.GetComponent<Player>();

                if ( IsWithinSelectionBounds( selectableObject.gameObject ) && player.human )
                {
                    SelectObject(selectableObject);
                }
                else
                {
                    if( selectableObject.selectionCircle != null )
                    {
                        Destroy( selectableObject.selectionCircle.gameObject );
                        selectableObject.selectionCircle = null;
                    }
                }
            }
        }
    }


    public void SelectObject(SelectableUnitComponent selectableObject)
    {
        if( selectableObject.selectionCircle == null )
        {
            selectableObject.selectionCircle = Instantiate( selectionCirclePrefab );
            selectableObject.selectionCircle.transform.SetParent( selectableObject.transform, false );
            selectableObject.selectionCircle.transform.eulerAngles = new Vector3( 90, 0, 0 );

            if (selectableObject.transform.GetComponent<ControlBasic>() != null)
            {
                selectableObject.transform.GetComponent<ControlBasic>().selected = true;
            }
        }
    }


    public bool IsWithinSelectionBounds( GameObject gameObject )
    {
        bool isFound = false;

        if( !isSelecting )
            return false;

        Camera camera = Camera.main;
        if (initialMousePos == Input.mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(initialMousePos);
            if(Physics.Raycast(ray, out hit))
            {
                if( hit.collider.gameObject == gameObject )
                    isFound = true;
            }
        }
        else
        {
            var viewportBounds = Utils.GetViewportBounds( camera, initialMousePos, Input.mousePosition );
            isFound = viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
        }
        return isFound;
    }

    void OnGUI()
    {
        if( isSelecting )
        {
            /*
             * We you click and drag, a rectangle appears that will select all units under it
             * The lines below draws that rectangle
             */
            var rect = Utils.GetScreenRect( initialMousePos, Input.mousePosition );
            Utils.DrawScreenRect( rect, new Color( 0.0f, 0.8f, 0.0f, 0.08f ) );
            Utils.DrawScreenRectBorder( rect, 2, Color.green );
        }
    }
}
