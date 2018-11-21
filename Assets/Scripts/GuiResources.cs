using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage the updating of the resources on the GUI
/// </summary>
public class GuiResources : MonoBehaviour
{
    Text SnowballCountText;
    Text SupplyCountText;

    void OnEnable()
    {
        SnowballCountText = GameObject.Find("SnowballCount").GetComponent<Text>();
        SupplyCountText = GameObject.Find("SupplyCount").GetComponent<Text>();

        EventManager.StartListening(EventManager.Events.SnowballUpdate, UpdateSnowballs);
        EventManager.StartListening(EventManager.Events.SupplyUpdate, UpdateSupply);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.SnowballUpdate, UpdateSnowballs);
        EventManager.StopListening(EventManager.Events.SupplyUpdate, UpdateSupply);
    }

    /// <summary>
    /// Add/Subtract snowballs
    /// </summary>
    /// <param name="snowballs"></param>
    void UpdateSnowballs(int snowballs)
    {
        SnowballCountText.text = snowballs.ToString();
    }

    /// <summary>
    /// Add/Subtract snowballs
    /// </summary>
    /// <param name="_gameObject">The calling gameobject (EventManager)</param>
    void UpdateSnowballs(GameObject _gameObject)
    {
        UpdateSnowballs(_gameObject.GetComponent<Resources>().GetSnowballCount());
    }

    /// <summary>
    /// Add/Subtract supply count
    /// </summary>
    /// <param name="supplyCount"></param>
    void UpdateSupply(int supplyCount)
    {
        SupplyCountText.text = supplyCount.ToString();
    }

    /// <summary>
    /// Add/Subtract supply count
    /// </summary>
    /// <param name="_gameObject">The calling gameobject (EventManager)</param>
    void UpdateSupply(GameObject _gameObject)
    {
        Resources R = _gameObject.GetComponent<Resources>();
        UpdateSupply(R.GetWorkersCount() + R.GetHeroCount());
    }
}
