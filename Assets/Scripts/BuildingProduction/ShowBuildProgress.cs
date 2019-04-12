using UnityEngine;

public class ShowBuildProgress : MonoBehaviour
{
    [SerializeField] Texture2D background;
    [SerializeField] Texture2D fillTexture;
    [SerializeField] BuildProgressSO buildingToPlace;
    [SerializeField] float width = 90f;
    [SerializeField] float height = 10f;

    public bool started { get; private set; }

    float startTime = -1f;
    float buildTime;

    void Start()
    {
        started = false;
        buildTime = buildingToPlace.currentBuilding.buildTime;
    }

    // called when a worker unit collides with the construction unit
    public void StartBuilding()
    {
        started = true;
        startTime = Time.time;
    }
	
	void OnGUI()
    {
        if (startTime == -1f)
            return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 guiPosition = new Vector2(screenPos.x - (width / 2), Screen.height - screenPos.y - height * 4);

        GUI.color = Color.white;
        GUI.BeginGroup(new Rect(guiPosition, new Vector2(width, height)));
        GUI.DrawTexture(new Rect(0, 0, width, height), background);

        GUI.BeginGroup(new Rect(0, 0, width * Mathf.Clamp01((Time.time - startTime) / buildTime), height));
        GUI.DrawTexture(new Rect(0, 0, width, height), fillTexture);

        GUI.EndGroup();
        GUI.EndGroup();

        GUI.contentColor = Color.black;
        int remaining = (int) Mathf.Ceil(buildTime - Time.time + startTime);
        GUI.Label(new Rect(guiPosition.x + width/2, guiPosition.y - height/2, width, 75), remaining.ToString());
    }
}
