using System.Collections;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

/* Attach this component to the main camera */
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
    /// Change MainCamera distance from view target.
    /// </summary>
    /// <param name="distance">Distance.</param>
    public void CameraDistance(int distance)
    {
        Transform trans = mainCamera.transform;
        if (trans.localPosition.y + distance >= nearCameraDist && trans.localPosition.y + distance <= farCameraDist)
        {
            trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y + distance, trans.localPosition.z - distance);
        }
    }

    /// <summary>
    /// Move the specified v.
    /// </summary>
    /// <param name="v">V.</param>
    public void Move(Vector3 v)
    {
        isCameraMoving = true;
        rb.velocity = (v * moveSpeed);
    }

    public void MoveCameraTo(Vector3 dest, float speed)
    {
        if ( lastLerpCameraTo != null )
            StopCoroutine(lastLerpCameraTo);

        lastLerpCameraTo = StartCoroutine( LerpCameraTo(dest,speed) );
    }

    IEnumerator LerpCameraTo(Vector3 dest, float speed)
    {
        float startTime = Time.time;
        Vector3 startPosition = mainCamera.transform.position;
        float journeyLength = Vector3.Distance(mainCamera.transform.position, dest);

        while (true)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            if (startPosition != dest)
                mainCamera.transform.position = Vector3.Lerp(startPosition, dest, fracJourney);
            else
                break;

            if (fracJourney >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
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

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
            CameraDistance( -1 );
        else if (scroll < 0)
            CameraDistance( 1 );
    }

    void Start()
    {
        mainCamera = Camera.main.gameObject;
        rb = mainCamera.GetComponent<Rigidbody>();
    }	
}
