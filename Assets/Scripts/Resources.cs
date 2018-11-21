using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resources (Units/Heros, Snowballs, etc)
/// </summary>
public class Resources : MonoBehaviour
{
    [SerializeField]
    private int Snowballs = 250;

    ///<remarks>Different Units/Heros.
    /// Add new unit types here
    ///</remarks>
    [SerializeField]
    private int Hero = 0;
    [SerializeField]
    private int Workers = 0;


    void Start()
    {
        EventManager.TriggerEvent(EventManager.Events.SnowballUpdate, gameObject);
        EventManager.TriggerEvent(EventManager.Events.SupplyUpdate, gameObject);
    }


    /// <summary> Gets the snowball count. </summary> <returns>The snowball count.</returns>
    public int GetSnowballCount() { return Snowballs; }

    /// <summary>Gets the workers count.</summary> <returns>The workers count.</returns>
    public int GetWorkersCount() { return Workers; }

    /// <summary> Gets the hero count.</summary> <returns>The hero count.</returns>
    public int GetHeroCount() { return Hero; }

    ///<summary>Snowball Management</summary>
    public void AddSnowballs()
    {
        Snowballs++;
        EventManager.TriggerEvent(EventManager.Events.SnowballUpdate, gameObject);
    }
    public void AddSnowballs(int number)
    {
        Snowballs += number;
        EventManager.TriggerEvent(EventManager.Events.SnowballUpdate, gameObject);
    }
    public void RemoveSnowballs(int numberUsed)
    {
        Snowballs -= numberUsed;
        EventManager.TriggerEvent(EventManager.Events.SnowballUpdate, gameObject);
    }

    /// <summary> Respawns/Kills the Hero. </summary>
    public void RespawnHero() { Hero++; }
    public void KillHero()
    {
        Hero--;
        EventManager.TriggerEvent(EventManager.Events.SupplyUpdate, gameObject);
    }
    
    /// <summary> Adds/Kills Workers. </summary>
    public void AddWorker(int WorkersBorn = 1)
    {
        Workers += WorkersBorn;
        EventManager.TriggerEvent(EventManager.Events.SupplyUpdate, gameObject);
    }
    public void KillWorker(int WorkersDied = 1)
    {
        Workers -= WorkersDied;
        EventManager.TriggerEvent(EventManager.Events.SupplyUpdate, gameObject);
    }
}
