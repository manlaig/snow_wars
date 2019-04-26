using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObject/Unit", order = 2)]
public class UnitSO : ScriptableObject
{
    public string objectName = "Unit Name";
    public GameObject unitPrefab;
    public float buildTime;
    public float cost;
    //public Icon icon;
}
