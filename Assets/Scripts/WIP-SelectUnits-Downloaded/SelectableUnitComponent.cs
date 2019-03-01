using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SelectableUnitComponent : MonoBehaviour
{
    [HideInInspector]
    // This gameObject is not null, if its currently selected
    public GameObject selectionCircle;
}