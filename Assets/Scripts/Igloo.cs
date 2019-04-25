using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Igloo Building Class.
/// </summary>
public class Igloo : BaseBuilding
{
    public override void OnClick()
    {
        Debug.Log("click");
    }

    public override void OnMouseHover()
    {
        Debug.Log("hover");
    }
}