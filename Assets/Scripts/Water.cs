using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Water Effects
/// </summary>
public class Water : MonoBehaviour
{
    Renderer rend;
    Vector2 offset;

    /// <summary>
    /// Water movement speed
    /// </summary>
    public float speed = 0.00025f;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaterPos();
    }

    /// <summary>
    /// Change the water texture offset
    /// </summary>
    void UpdateWaterPos()
    {
        offset = rend.material.GetTextureOffset("_MainTex");
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset.x + speed, offset.y + speed));
    }
}