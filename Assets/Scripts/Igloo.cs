using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Igloo Building Class.
/// </summary>
public class Igloo : BaseBuilding
{
    [SerializeField] Transform unitProduce;
    [SerializeField] GameObject iglooGUI;
    [SerializeField] Texture2D background;
    [SerializeField] Texture2D fillTexture;

    float width = 120f;
    float height = 15f;
    float buildTime;
    float startTime;

    GameObject player;

    bool working = false;

    public override void OnClick()
    {
        if(gameObject.activeInHierarchy)
            iglooGUI.SetActive(true);
    }

    public override void OnMouseHover()
    {
    }

    public void MakeUnit(UnitSO unit)
    {
        if (working) return;
        if(!player)
        {
            player = FindObjectOfType<Player>().gameObject;
        }
        StartCoroutine(MakeUnitCoroutine(unit));
    }

    IEnumerator MakeUnitCoroutine(UnitSO unit)
    {
        if (!player)
        {
            Debug.Log("Instance of Player not found in scene");
            yield break;
        }
        buildTime = unit.buildTime;
        startTime = Time.time;
        working = true;
        yield return new WaitForSeconds(buildTime);
        GameObject go = Instantiate(unit.unitPrefab, player.transform);
        go.transform.position = unitProduce.position;
        working = false;
    }

    void OnGUI()
    {
        if(working)
        {
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
            int remaining = (int)Mathf.Ceil(buildTime - Time.time + startTime);
            GUI.Label(new Rect(guiPosition.x + width / 2, guiPosition.y, width, 75), remaining.ToString());
        }
    }
}