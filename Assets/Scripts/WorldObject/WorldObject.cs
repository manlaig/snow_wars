using UnityEngine;

/// <summary>
/// World Object Class
/// </summary>
public class WorldObject : MonoBehaviour
{
    public int cost;

    protected Player player;
    protected string[] actions = { };
    protected bool currentlySelected = false;


    public void SetSelection(bool selected)
    {
        currentlySelected = selected;
    }

    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        Debug.Log("mouse click");
        //only handle input if currently selected
        if (currentlySelected && hitObject && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
            //clicked on another selectable object
            if (worldObject)
                ChangeSelection(worldObject, controller);
        }
    }

    private void ChangeSelection(WorldObject worldObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        SetSelection(false);
        if (controller.SelectedObject)
            controller.SelectedObject.SetSelection(false);
        controller.SelectedObject = worldObject;
        worldObject.SetSelection(true);
    }

    public string[] GetActions()
    {
        return actions;
    }

    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnGUI()
    {

    }
}
