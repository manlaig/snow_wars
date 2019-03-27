using UnityEngine;

/// <summary> //UPDATE ME//
/// This is the base class for all workers and hero classes.
/// Current Private Variables are;
///  health - 100 - Float
///  mana - 100 - Float
///  isUnitEnabled - false - bool
/// </summary>
public class Unit : WorldObject
{
    public int PRIORITY = -1;

    [SerializeField]
    protected float fullHealth = 100;
    [SerializeField]
    protected float fullMana = 100;
    [SerializeField]
    protected float damagePerHit = 0;
    [SerializeField]
    protected float attackHitDelay = 0;
    //[SerializeField]
    //protected float attackRecharge = 0;
    [SerializeField]
    protected Sprite icon;

    protected float health;
    protected float mana;
    protected bool isUnitKilled = false;

    /// <summary>
    /// Gets the icon.
    /// </summary>
    /// <returns>The icon.</returns>
    public Sprite GetUnitIcon()
    {
        return icon;
    }

    /// <summary>
    /// Gets the health of character.
    /// </summary>
    /// <returns>The health.</returns>
    public float GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Gets the health of character.
    /// </summary>
    /// <returns>The health.</returns>
    public float GetMaxHealth()
    {
        return fullHealth;
    }

    /// <summary>
    /// Gets the Unit health as a percent
    /// </summary>
    /// <returns>Health as a percent (100% = 1f)</returns>
    public float GetHealthPercent()
    {
        return health / fullHealth;
    }

    /// <summary>
    /// Gets the mana of character
    /// </summary>
    /// <returns>The mana.</returns>
    public float GetMana()
    {
        return mana;
    }

    /// <summary>
    /// Gets the amount of damage the Unit can do
    /// </summary>
    /// <returns>Amount of damage per hit</returns>
    public float GetDamagePerHit()
    {
        return damagePerHit;
    }

    /// <summary>
    /// Get the amount of time before the Unit can attack again
    /// </summary>
    /// <returns>Amount of time before next attack in seconds</returns>
    /*
    public float GetAttackRecharge()
    {
        return attackRecharge;
    }
    */

    /// <summary>
    /// The time till the animation attack hits
    /// </summary>
    /// <returns>Time in seconds</returns>
    public float GetHitDelay()
    {
        return attackHitDelay;
    }

    /// <summary>
    /// Returns the snowball cost.
    /// </summary>
    /// <returns>Snowball Cost.</returns>
    public int ReturnSnowBallCost()
    {
        return cost;
    }

    /// <summary>
    /// Set the unit cost
    /// </summary>
    /// <param name="snowballCost">The amount of snowballs for the unit.</param>
    public void SetSnowballCost(int snowballCost = 5)
    {
        cost = snowballCost;
    }

    /// <summary>
    /// subtracts health from unit.
    /// </summary>
	public void TakeDamage(GameObject attacker, float damage = 10)
    {
        Animator animation = gameObject.GetComponent<Animator>();
        animation.SetTrigger("Hit");

        health -= damage;
        //Debug.Log(this.name + " health = " + health);

        // Check if Unit exists
        if (gameObject)
        {
            // Broadcast a health update to listening scripts
            EventManager.TriggerEvent(EventManager.Events.HealthUpdate, gameObject);

            if(health <= 0 && !isUnitKilled)
            {
                isUnitKilled = true;
                // Broadcast a Unit killed to listening scripts
                EventManager.TriggerEvent(EventManager.Events.UnitKilled, gameObject);

                // Play Killed Animation and remove gameObject
                animation.SetTrigger("Killed");

                // Disable Collier to stop attacks
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void KilledEvent()
    {
        Destroy(gameObject, 0); // TODO: Give an option to delay the destroy
    }


    /// <summary>
    /// Set health of Unit
    /// </summary>
    /// <param name="unitHealth">Unit Health</param>
    public void SetHealth(float unitHealth)
    {
        health = unitHealth;
    }

    /// <summary>
    /// Uses the mana.
    /// </summary>
    public void UseMana(float manaCast = 10)
    {
        mana -= manaCast;
    }

    public void SetMana(float unitMana)
    {
        mana = unitMana;
    }

    /*** Game Engine methods, all can be overridden by subclass ***/

    protected override void Awake()
    {
        base.Awake();

        health = fullHealth;
        mana = fullMana;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnGUI()
    {
        base.OnGUI();
    }
}
