﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarLighter : MonoBehaviour
{
    [Tooltip("The radius which this gameObject can light the fog plane")]
    [SerializeField] float radius = 75f;
    [Tooltip("The current fog plane in the scene, not the prefab in the assets")]
    [SerializeField] GameObject fogPlane;

    void Awake()
    {
        if (!fogPlane)
            Debug.LogError("Need a reference to the current fog plane. If you don't have any, disable this component");
    }

    // TODO: create an event when mouse 2 is clicked and update the fog plane in response to the event
    void Update ()
    {
        fogPlane.GetComponent<FogPlane>().UpdateColor(transform, radius);
	}
}
