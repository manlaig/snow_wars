using UnityEngine;

/// <summary>
/// Displays a health bare above a unit
/// </summary>
public class UnitHealthBar : MonoBehaviour
{
    [SerializeField]
    private Texture2D goodHealth;
    [SerializeField]
    private Texture2D warningHealth;
    [SerializeField]
    private Texture2D criticalHealth;
    [SerializeField]
    private Texture2D background;
    [SerializeField]
    private Color opacity = Color.white;
    [SerializeField]
    private float height;
    [SerializeField]
    private float width;
    [SerializeField]
    private float timeDelay;

    private Unit unit;
    private float timeStart;
    private float timeProgress;
    private float lerpProgress;
    private float lastHealth;
    private float displayedHealth;

    // Use this for initialization
    void Awake()
    {
        unit = transform.parent.GetComponent<Unit>();
    }

    void OnEnable()
    {
        // Prime the displyedHealth and lastHealth
        lastHealth = displayedHealth = unit.GetHealthPercent();
    }

    // Update is called once per frame
    void OnGUI()
    {
        // TODO: Make toggleable
        // Display healthbar when unit is enabled
        if (unit.enabled) UpdateHeathBar();
    }

    /// <summary>
    /// Update the unit's health bar.
    /// </summary>
    void UpdateHeathBar()
    {
        float health = unit.GetHealthPercent();

        // Check if health has changed and is not dead
        if (health != displayedHealth && health > 0f)
        {
            // If the unit health is different from lastHealth
            if (health != lastHealth)
            {
                timeStart = Time.time;
                // Setup the time progress and cancel out the first deltaTime
                timeProgress = timeStart - Time.deltaTime;
            }

            // Smoothly change health to new value using timeDelay to set speed
            timeProgress += Time.deltaTime;
            lerpProgress = timeProgress / (timeStart + timeDelay);
            displayedHealth = Mathf.Lerp(displayedHealth, health, Mathf.Clamp01(lerpProgress));
        }

        // Display the current or updated health
        DrawHealth(displayedHealth);

        // Keep track of health for next update
        lastHealth = health;
    }

    /// <summary>
    /// Set the new Health Bar value.
    /// </summary>
    /// <param name="health">A value from 0f to 1f</param>
    void DrawHealth(float health)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 guiPosition = new Vector2(screenPos.x - (width / 2), Screen.height - screenPos.y - height);

        GUI.color = opacity;
        GUI.BeginGroup(new Rect(guiPosition, new Vector2(width, height)));
        GUI.DrawTexture(new Rect(0, 0, width, height), background);

        GUI.BeginGroup(new Rect(0, 0, width * Mathf.Clamp01(health), height));
        GUI.DrawTexture(new Rect(0, 0, width, height), GetHealthTex(health));

        GUI.EndGroup();
        GUI.EndGroup();
    }

    /// <summary>
    /// Get the health bar texture(color) for the current health.
    /// </summary>
    /// <param name="health">A value from 0f to 1f</param>
    /// <returns>Health Bar Texture</returns>
    Texture2D GetHealthTex(float health)
    {
        Texture2D texture;

        // Check if health is over 2/3
        if (health > 0.6666f)
            texture = goodHealth;
        // Check if health if over 1/3
        else if (health > 0.3333f)
            texture = warningHealth;
        else
            texture = criticalHealth;

        return texture;
    }
}