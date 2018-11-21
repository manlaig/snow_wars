using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionRotate : MonoBehaviour
{

    float rotationsPerMinute = 10.0f;

    void Start()
    {
        transform.Rotate(0, 0, Random.value * 360);
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationsPerMinute * Time.deltaTime);
    }
}
