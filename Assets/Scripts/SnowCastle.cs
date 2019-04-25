using UnityEngine;

/// <summary>
/// SnowCastle Class
/// <summary>
public class SnowCastle : BaseBuilding
{
    protected GameObject coreObject;

    /// <summary>
    /// Called at start of game
    /// </summary>
    new protected void Start()
    {
        workerCapacity = 5;  // Undetermined Value
        base.Start();
    }

    /// <summary>
    /// Takes the damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    new public void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
        {
            EventManager.TriggerEvent(EventManager.Events.CastleDistroyed);
        }
    }

    public override void OnClick()
    {
        
    }
}
