using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MiniMap logic script and config
/// </summary>
public class MiniMap : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    private Camera minimapCamera;
    private Camera mainCamera;
    private Canvas guiCanvas;
    private UserInput userInput;
    private Vector2 screenSize;

    private void Awake()
    {
        // Keep track of screen height and width when script is enabled
        screenSize = new Vector2(Screen.width, Screen.height);

        // Create object links
        minimapCamera = GetComponentsInChildren<Camera>()[0];
        mainCamera = Camera.main;
        userInput = GameObject.Find("Player").GetComponent<UserInput>();
        guiCanvas = gameObject.transform.parent.gameObject.GetComponent<Canvas>();
    }

    // Use this for initialization
    void Start()
    {
        // Update reder texture on start
        StartCoroutine(UpdateRenderTextureSize());

        if (minimapCamera == null || mainCamera == null)
            Debug.LogError("Minimap and/or Main Camera not set in Inspector");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the screen height or width has changed
        if (screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            StartCoroutine(UpdateRenderTextureSize());

            // Update the stored screen size
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        // Check if the user is left clicking on the mini-map
        if (Input.GetMouseButtonDown(0) && minimapCamera.pixelRect.Contains(Input.mousePosition))
        {
            RaycastHit hit;
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Get the current camera height
                float cameraHeight = mainCamera.transform.localPosition.y;

                // Move camera to new location at a user settable speed.
                userInput.MoveCameraTo(new Vector3(hit.point.x, cameraHeight, hit.point.z - cameraHeight), cameraSpeed);
            }
        }
    }

    /// <summary>
    /// Updates the size of the render texture.
    /// </summary>
    private IEnumerator UpdateRenderTextureSize()
    {
        yield return new WaitForEndOfFrame();

        // Get the size of the RectTransform
        Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
        // Get the Canvas scale
        Vector2 canvasScale = new Vector2(guiCanvas.transform.localScale.x, guiCanvas.transform.localScale.y);
        // Calculate the pixel dimensions for the Render Texture
        Vector2 finalScale = new Vector2(sizeDelta.x * canvasScale.x, sizeDelta.y * canvasScale.y);

        // Create new render texture using the new size and replace the original.
        RenderTexture rendTexture = new RenderTexture((int)finalScale.x, (int)finalScale.y, 24);
        minimapCamera.targetTexture = rendTexture;
        gameObject.GetComponent<RawImage>().texture = rendTexture;
    }
}
