/// <summary>
/// Video settings.
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    /// <summary>
    /// Sets the fullscreen to the boolean passed to it.
    /// </summary>
    /// <param name="selected">If set to <c>true</c> selected.</param>
    public void SetFullscreen(bool selected)
    {
        Screen.fullScreen = selected;
    }

    void Start()
    {
        Toggle fullscreenToggle = GetComponent<Toggle>();
        fullscreenToggle.isOn = Screen.fullScreen;
    }
}
