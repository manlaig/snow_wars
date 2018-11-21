using UnityEngine;

/// <summary>
/// Exit on click.
/// </summary>
public class ExitOnClick : MonoBehaviour
{
    /// <summary>
    /// Exit the game.
    /// </summary>
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
		    //Application.Exit();
		    Application.Quit();
        #endif
    }
}
