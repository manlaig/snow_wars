using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Class
/// </summary>
public class Player : MonoBehaviour
{
    public string username;
    public bool human;
    public WorldObject SelectedObject;

    protected bool isDead = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsPlayerDead()
    {
        return isDead;
    }
}