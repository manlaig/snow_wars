using UnityEngine;

public class SelectableUnitComponent : MonoBehaviour
{
    [HideInInspector]
    // This gameObject is not null, if its currently selected
    public GameObject selectionCircle;
}