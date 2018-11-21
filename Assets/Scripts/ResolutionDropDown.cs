using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

/// <summary>
/// Resolution drop down.
/// Allows Resolution drop down to be filled with valid resolutions.
/// Adds Listenter to implement resolution change.
/// </summary>
public class ResolutionDropDown : MonoBehaviour
{
    public Dropdown resDropdown;

    Resolution[] resolutions;

    // Use this for initialization
    void Start()
    {
        resDropdown = GetComponent<Dropdown>();
        resolutions = Screen.resolutions;
        resDropdown.options.Clear();

        /// Populates drop down with possible resolutions
        /// And Adds listener to each
        foreach (Resolution option in resolutions)
        {
            resDropdown.options.Add(new Dropdown.OptionData(option.ToString()));
            resDropdown.onValueChanged.AddListener(delegate {
                Screen.SetResolution(resolutions[resDropdown.value].width, resolutions[resDropdown.value].height, Screen.fullScreen);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
