using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Enable Navigation by keyboard or gamepad.
/// </summary>
public class SelectOnInput : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /// Vertical arrow keys or gamepad select menu buttons for navigation.
        if (Input.GetAxisRaw("Vertical") != 0 && !buttonSelected)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    /// <summary>
    /// Deselect Disabled button.
    /// </summary>
    private void OnDisable()
    {
        buttonSelected = false;
    }
}
