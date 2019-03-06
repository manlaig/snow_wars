using UnityEngine;

/// <summary>
/// In game menu toggle.
/// Show and hide in game menu.
/// </summary>
public class InGameMenuToggle : MonoBehaviour
{
    [SerializeField]
    private Transform mainPanel;
    [SerializeField]
    private Transform audioPanel;
    [SerializeField]
    private Transform videoPanel;

    public bool IsMenuActive()
    {
        return (mainPanel.gameObject.activeSelf ||
                audioPanel.gameObject.activeSelf ||
                videoPanel.gameObject.activeSelf
               );
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
