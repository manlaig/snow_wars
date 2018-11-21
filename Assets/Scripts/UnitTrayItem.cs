using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Link a Unit Tray prefab to a unit.
/// </summary>
public class UnitTrayItem : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image healthBar;
    private Unit unitLink;

    void Start()
    {
        // Fix GUI Position in Selection Tray
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    void OnEnable()
    {
        EventManager.StartListening(EventManager.Events.HealthUpdate, SetHealth);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.HealthUpdate, SetHealth);
    }

    public void SetupUnitLink(Unit unit)
    {
        // Create links to the unit
        unitLink = unit;
        SetIcon(unitLink.GetUnitIcon());
        SetHealth(unitLink.GetHealthPercent());
    }

    /// <summary>
    /// Set the unit icon
    /// </summary>
    /// <param name="unitIcon">Unit icon to use</param>
    void SetIcon(Sprite unitIcon)
    {
        // Update the unit icon
        icon.sprite = unitIcon;
    }

    /// <summary>
    /// Set the health bar fill
    /// </summary>
    /// <param name="healthPercent">Health from 0f to 1f</param>
    void SetHealth(float healthPercent)
    {
        healthBar.fillAmount = Mathf.Clamp01(healthPercent);
    }

    /// <summary>
    /// Set the display health
    /// </summary>
    /// <param name="unitGO">Unit that is to be updated</param>
    void SetHealth(GameObject unitGO)
    {
        // Check if it is the correct unit
        if (unitGO.Equals(unitLink.gameObject))
            SetHealth(unitLink.GetHealthPercent());
    }

    public void SelectUnit()
    {
        SelectableUnitComponent suc = unitLink.gameObject.GetComponent<SelectableUnitComponent>();

        FindObjectOfType<UnitSelectionComponent>().SelectObject(suc);
    }
}
