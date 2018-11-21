using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    [SerializeField] float speed = 5f;
	void Update ()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.Translate(x * speed * Time.deltaTime, 0, y * speed * Time.deltaTime);
	}
}
