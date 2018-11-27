using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOnScroll : MonoBehaviour
{
    [SerializeField] float sensitivity = 1f;
    [SerializeField] float minSize = 10f;
    [SerializeField] float maxSize = 50f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
            CameraDistance(-1);
        else if (scroll < 0)
            CameraDistance(1);
    }

    public void CameraDistance(int distance)
    {
        float newSize = Camera.main.orthographicSize + distance * sensitivity;
        if (newSize < maxSize && newSize > minSize)
            Camera.main.orthographicSize = newSize;
    }
}
