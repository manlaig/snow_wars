using System.Collections;
using UnityEngine;

public class SnowAngel : MonoBehaviour
{
    [SerializeField] float healRadius;
    [SerializeField] float healthAmount;
    [Tooltip("The frequency of heal")]
    [SerializeField] float healFrequency;

    void Start()
    {
        StartCoroutine(healCoroutine());
    }

    IEnumerator healCoroutine()
    {
        while(true)
        {
            HealInRadius();
            yield return new WaitForSeconds(healFrequency);
        }
    }

    // Heal units in the heal radius with heal amount
    void HealInRadius()
    {
        // Get all colliders in the heal radius
        Collider[] colliderInRadius = Physics.OverlapSphere(transform.position, healRadius);

        foreach(Collider col in colliderInRadius)
        {
            // check if collider is a unit
            Unit unit = col.gameObject.GetComponent<Unit>();

            if (unit && unit.GetHealth() < unit.GetMaxHealth())
            {
                unit.SetHealth(Mathf.Clamp(unit.GetHealth() + healthAmount, 0, unit.GetMaxHealth()));
                //Debug.Log("Unit: " + unit.name + " health: " + unit.GetHealth());
            }

        }
    }
}
