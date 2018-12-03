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
    Vector3 mousePosition1;

    public GameObject selectionCirclePrefab;
    public GameObject guiController;
    private InGameMenuToggle menuInGame;


    private void OnEnable()
    {
        EventManager.StartListening(EventManager.Events.ClearSelection, ClearSelection);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.ClearSelection, ClearSelection);
    }

    private void ClearSelection(GameObject callback)
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
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if( Input.GetMouseButtonDown( 0 ) && !isMenuActive() && !EventSystem.current.IsPointerOverGameObject() )
        {
            isSelecting = true;

            mousePosition1 = Input.mousePosition;

            foreach( var selectableObject in FindObjectsOfType<SelectableUnitComponent>() )
            {
                if( selectableObject.selectionCircle != null )
                {
                    Destroy( selectableObject.selectionCircle.gameObject );
                    selectableObject.selectionCircle = null;
                    selectableObject.GetComponent<ControlBasic>().selected = false;
                }
            }
        }
        // If we let go of the left mouse button, end selection
        if( Input.GetMouseButtonUp( 0 ) && !isMenuActive() )
        {
            // TODO: this block is being executed every time left mouse is pressed
            // FindObjectsOfType is very slow, this block is running it every left mouse press
            var selectedObjects = new List<SelectableUnitComponent>();
            List<GameObject> selectedItems = new List<GameObject>();
            foreach( var selectableObject in FindObjectsOfType<SelectableUnitComponent>() )
            {
                // TO DO: Find better way for excluding non-player
                Player player = selectableObject.gameObject.transform.root.GetComponent<Player>();

                if( IsWithinSelectionBounds( selectableObject.gameObject ) && player.human )
                {
                    selectableObject.GetComponent<ControlBasic>().selected = true;
                    selectedObjects.Add( selectableObject );
                    selectedItems.Add(selectableObject.gameObject);
                }
            }

            if (selectedObjects.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
                foreach (var selectedObject in selectedObjects)
                {
                    sb.AppendLine("-> " + selectedObject.gameObject.name);
                }
                Debug.Log(sb.ToString());
            }

            guiController.GetComponent<GuiController>().SetSelectedObjects(selectedItems);
            isSelecting = false;
        }
        else if (Input.GetMouseButtonUp( 0 ))
        {
            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if( isSelecting )
        {
            //singleSelection = false;
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
            //singleSelection = true;
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
        /*if (singleSelection == true)
        {
            List<GameObject> tempList = new List<GameObject>();
            tempList.Add(selectableObject.gameObject);
            G.SetSelectedObjects(tempList);
        }*/
    }


    public bool IsWithinSelectionBounds( GameObject gameObject )
    {
        bool isFound = false;

        if( !isSelecting )
            return false;

        var camera = Camera.main;
        if (mousePosition1 == Input.mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition1);
            if(Physics.Raycast(ray, out hit))
            {
                if( hit.collider.gameObject == gameObject )
                    isFound = true;
            }
        }
        else
        {
            var viewportBounds = Utils.GetViewportBounds( camera, mousePosition1, Input.mousePosition );
            isFound = viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
        }
        return isFound;
    }

    void OnGUI()
    {
        if( isSelecting )
        {
            Debug.Log("is selecting true");
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect( mousePosition1, Input.mousePosition );
            Utils.DrawScreenRect( rect, new Color( 0.0f, 0.8f, 0.0f, 0.08f ) );
            Utils.DrawScreenRectBorder( rect, 2, Color.green );
        }
    }
}
