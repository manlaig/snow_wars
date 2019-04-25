using System.Collections;
using UnityEngine;

/// <summary>
/// Base Building Class.
/// </summary>
public abstract class BaseBuilding : MonoBehaviour
{
    [SerializeField] // Exposes Protected Variable to the Inspector
    protected float fullHealth = 100;  // Unconfirmed Value
    [SerializeField]
    protected int repairCost = 5;  // Unconfirmed Value
    [SerializeField]
    protected float repairAmount = 10; // Unconfirmed Value
    [SerializeField]
    protected int workerCapacity = 1; // Unconfirmed Value
    [SerializeField]
    protected GameObject destroyedBuilding; // Optional
    [SerializeField]
    protected GameObject upgradedBuilding; // Optional
    [SerializeField]
    protected Resources R; // Required

    [SerializeField]
    protected float health;
    protected bool isRepairing = false;
    protected float repairTime = 1; // Repair delay time in seconds

    protected BaseBuilding()
    {
        health = fullHealth;
    }

    /// <summary>
    /// Gets the building health.
    /// </summary>
    /// <returns>The building health.</returns>
    public float GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Determines whether this building is upgradeable.
    /// </summary>
    /// <returns><c>true</c> if this building is upgradeable; otherwise, <c>false</c>.</returns>
    public bool IsUpgradable()
    {
        return (upgradedBuilding != null);
    }

    /// <summary>
    /// Repair this building.
    /// </summary>
    public void Repair()
    {
        isRepairing = true;
        StartCoroutine(Repairing());
    }

    /// <summary>
    /// Takes the damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void TakeDamage(float damage = 10)
    {
        health -= damage;

        if (health <= 0)
        {
            DistroyBuilding();
        }
    }

    /// <summary>
    /// Start repairing the building with time delay.
    /// </summary>
    protected IEnumerator Repairing()
    {
        while (repairCost <= R.GetSnowballCount() &&
            health <= fullHealth &&
            isRepairing)
        {
            R.RemoveSnowballs(repairCost);
            health += repairAmount;

            if (health >= fullHealth)
            {
                health = fullHealth;
                break;
            }

            yield return new WaitForSeconds(repairTime);
        }
        isRepairing = false;
    }

    /// <summary>
    /// Destroys the building.
    /// </summary>
    protected void DistroyBuilding()
    {
        if (destroyedBuilding != null)
        {
            Instantiate(destroyedBuilding, transform.position, transform.rotation);
        }

        for (int i = 0; i < workerCapacity; i++)
        {
            R.KillWorker(); // Need to add a parameter for number of workers
        }

        Destroy(gameObject); // Destroy attached game object
    }

    /// <summary>
    /// On start script initialization.
    /// </summary>
    protected void Start()
    {
        health = fullHealth;

        //if (resourcesObject == null)
        //    resourcesObject = transform.root.GetComponent<Resources>(); // Temp - Need to (should) ref the RC to this script in the Inspector
        //if (resourcesObject == null)
        //    Debug.LogError("Unable to find 'ResourcesController'");

        /*if (R != null)
            R.AddWorker(workerCapacity); // Need to add a parameter for number of workers
        else
            Debug.Log("Resources object is NULL");*/
    }

    // the events require a GameObject parameter, but we'll not use it
    public abstract void OnClick();
    public abstract void OnMouseHover();
}