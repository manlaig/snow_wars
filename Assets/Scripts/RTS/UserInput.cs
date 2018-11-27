using System.Collections;
using UnityEngine;
using RTS;

/// <summary>
/// User Input Class
/// </summary>
public class UserInput : MonoBehaviour
{
    private Player player;
    private Coroutine lastLerpCameraTo;

    void Start()
    {
        player = transform.root.GetComponent<Player>();
    }

    void Update()
    {
        if (player.human)
        {
            MoveCamera();
            MouseActivity();
        }
    }

    /// <summary>
    /// Handle any mouse activity
    /// </summary>
    private void MouseActivity()
    {
        if (Input.GetButtonDown("Fire1"))
            LeftMouseClick();
        else if (Input.GetButtonDown("Fire2"))
            RightMouseClick();
    }

    /// <summary>
    /// Handle left mouse click
    /// </summary>
    private void LeftMouseClick()
    {
        //if (player.hud.MouseInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject)
                    player.SelectedObject.MouseClick(hitObject, hitPoint, player);
                else if (hitObject.name != "Ground")
                {
                    WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
                    if (worldObject)
                    {
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handle right mouse click
    /// </summary>
    private void RightMouseClick()
    {
        if (/*player.hud.MouseInBounds() &&*/ !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false);
            player.SelectedObject = null;
        }
    }

    /// <summary>
    /// Find the object under the mouse
    /// </summary>
    /// <returns>The GameObject</returns>
    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;
        return null;
    }

    /// <summary>
    /// Find the position of the mouse in world/3d space
    /// </summary>
    /// <returns>The mouse position</returns>
    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;
        return ResourceManager.InvalidPosition;
    }

    /// <summary>
    /// Handle camera movement from detected user input
    /// </summary>
    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        //horizontal camera movement
        if ((xpos >= 0 && xpos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.A))
        {
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if ((xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.D))
        {
            movement.x += ResourceManager.ScrollSpeed;
        }

        //vertical camera movement
        if ((ypos >= 0 && ypos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.S))
        {
            movement.z -= ResourceManager.ScrollSpeed;
        }
        else if ((ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.W))
        {
            movement.z += ResourceManager.ScrollSpeed;
        }


        ////make sure movement is in the direction the camera is pointing
        ////but ignore the vertical tilt of the camera to get sensible scrolling
        //movement = Camera.mainCamera.transform.TransformDirection(movement);
        //movement.y = 0;

        //calculate desired camera position based on received input
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //limit away from ground movement to be between a minimum and maximum distance
        if (destination.y > ResourceManager.MaxCameraHeight)
        {
            destination.z = origin.z + (origin.y - ResourceManager.MaxCameraHeight);
            destination.y = ResourceManager.MaxCameraHeight;
        }
        else if (destination.y < ResourceManager.MinCameraHeight)
        {
            destination.z = origin.z + (origin.y - ResourceManager.MinCameraHeight);
            destination.y = ResourceManager.MinCameraHeight;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }
    }

    /// <summary>
    /// Move camera to destination over time
    /// </summary>
    /// <param name="dest">Destination</param>
    /// <param name="speed">Speed from 0f to 1f</param>
    /// <returns></returns>
    public void MoveCameraTo(Vector3 dest, float speed)
    {
        if (lastLerpCameraTo != null)
            StopCoroutine(lastLerpCameraTo);

        lastLerpCameraTo = StartCoroutine(LerpCameraTo(dest, speed));
    }

    /// <summary>
    /// Move camera to destination over time
    /// </summary>
    /// <param name="dest">Destination</param>
    /// <param name="speed">Speed from 0f to 1f</param>
    /// <returns></returns>
    IEnumerator LerpCameraTo(Vector3 dest, float speed)
    {
        float startTime = Time.time;
        Vector3 startPosition = Camera.main.transform.position;
        float journeyLength = Vector3.Distance(Camera.main.transform.position, dest);

        while (true)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            if (startPosition != dest)
                Camera.main.transform.position = Vector3.Lerp(startPosition, dest, fracJourney);
            else
                break;

            if (fracJourney >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
    }
}
