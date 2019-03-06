﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPlane : MonoBehaviour
{
    // Make sure the fog place is in a separate layer than other gameobject in the scene
    [SerializeField] LayerMask fogOfWarLayer;
    Vector3[] vertices;
    Color[] colors;

    void Start()
    {
        vertices = GetComponent<MeshFilter>().mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.black;
        }
        GetComponent<MeshFilter>().mesh.colors = colors;
    }

    public void UpdateColor(Transform t, float radius)
    {
        Ray ray = new Ray(Camera.main.transform.position, t.position - Camera.main.transform.position);
        RaycastHit hit;

        // raycasting and finding the intersection between fog plane and camera
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, fogOfWarLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = transform.TransformPoint(vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < radius * radius)
                {
                    float alpha = Mathf.Min(colors[i].a, dist / radius / radius);
                    colors[i].a = alpha;
                }
            }
            GetComponent<MeshFilter>().mesh.colors = colors;
        }
    }
}
