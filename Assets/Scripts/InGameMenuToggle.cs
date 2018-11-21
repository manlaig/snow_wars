using UnityEngine;

/// <summary>
/// In game menu toggle.
/// Show and hide in game menu.
/// </summary>
public class InGameMenuToggle : MonoBehaviour
{
    private GameObject menuCanvas;
    private Transform mainPanel;
    private Transform audioPanel;
    private Transform videoPanel;

    public bool IsMenuActive()
    {
        return (mainPanel.gameObject.activeSelf ||
                audioPanel.gameObject.activeSelf ||
                videoPanel.gameObject.activeSelf
               );
    }

    // Use this for initialization
    void Start()
    {
        menuCanvas = gameObject;
        mainPanel = menuCanvas.transform.Find("mainDropPanel");
        audioPanel = menuCanvas.transform.Find("audioDropPanel");
        videoPanel = menuCanvas.transform.Find("videoDropPanel");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // Toggle In Game Menu ON and Off
    public void ToggleMenu()
    {
        bool isShown = (mainPanel.gameObject.activeSelf || audioPanel.gameObject.activeSelf || videoPanel.gameObject.activeSelf);
        if (isShown)
        {
            mainPanel.gameObject.SetActive(false);
            audioPanel.gameObject.SetActive(false);
            videoPanel.gameObject.SetActive(false);
        }
        else
        {
            mainPanel.gameObject.SetActive(true);
        }
    }
}
