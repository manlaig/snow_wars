using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load scene on click.
/// </summary>
public class LoadSceneOnClick : MonoBehaviour
{
    /// <summary>
    /// Loads new Scene by Index number
    /// </summary>
    /// <param name="sceneIndex">Scene index.</param>
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
