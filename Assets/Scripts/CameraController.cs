using System.Collections;
using UnityEngine;
/*
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
*/
/* Attach this component to the main camera */
/*
public class CameraController : MonoBehaviour
{
    [SerializeField]
    public enum WindowEdges { Left, Right, Top, Bottom, LeftTop, LeftBottom, RightTop, RightBottom }

    private GameObject mainCamera;
    [SerializeField]
    private Boundary boundary;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float nearCameraDist = 10, farCameraDist = 50;

    private Coroutine lastLerpCameraTo;
    private bool isCameraMoving = false;
    private Rigidbody rb;

    /// <summary>
    /// Move the specified v.
    /// </summary>
    /// <param name="v">V.</param>
    public void Move(Vector3 v)
    {
        isCameraMoving = true;
        rb.velocity = (v * moveSpeed);
    }
    
    /// <summary>
    /// Cancel any active camera movement via rigidbody.
    /// </summary>
    public void CancelMove()
    {
        rb.velocity = Vector3.zero;
        isCameraMoving = false;
    }

    void FixedUpdate()
    {
        if (!isCameraMoving)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            rb.velocity = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;
        }

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
            rb.position.y,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
    }

    void Start()
    {
        mainCamera = Camera.main.gameObject;
        rb = mainCamera.GetComponent<Rigidbody>();
    }	
}
*/