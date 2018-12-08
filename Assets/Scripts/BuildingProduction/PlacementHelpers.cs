using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// helper methods for building placement
public static class PlacementHelpers
{
	public static bool RaycastFromMouse(out RaycastHit h, LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out h, Mathf.Infinity, layer))
        {
            return true;
        }
        return false;
    }


    public static void ToggleRenderers(GameObject go, bool toggle)
    {
        if (!go)
            return;
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach(Renderer r in renderers)
                r.enabled = toggle;
        }
    }


	public static Rect MakeRectOfCollider(Collider col)
	{
		Rect r = new Rect(col.bounds.center.x - col.bounds.extents.x,
						col.bounds.center.z - col.bounds.extents.z,
						col.bounds.size.x, col.bounds.size.z);
		return r;
	}
}
