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
    protected float fullHealth = -1;
    [SerializeField]
    protected float fullMana = -1;
    [SerializeField]
    protected float damagePerHit = -1;
    [SerializeField]
    protected float attackHitDelay = -1;
    [SerializeField]
    protected float attackRecharge = -1;
    [SerializeField]
    protected bool isUnitEnabled = false;
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
    public float GetAttackRecharge()
    {
        return attackRecharge;
    }

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
    /// Returns the unit status of character.
    /// </summary>
    /// <returns><c>true</c>, if unit status was returned, <c>false</c> otherwise.</returns>
    public bool ReturnUnitStatus()
    {
        return isUnitEnabled;
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
        health -= damage;
        //Debug.Log(this.name + " health = " + health);

        // Check if Unit is still alive
        if (health > 0 && gameObject != null)
            // Broadcast a health update to listening scripts
            EventManager.TriggerEvent(EventManager.Events.HealthUpdate, gameObject);
        else if (!isUnitKilled)
        {
            isUnitKilled = true;
            // Broadcast a Unit killed to listening scripts
            EventManager.TriggerEvent(EventManager.Events.UnitKilled, gameObject);

            // Play Killed Animation and remove gameObject
            Animation animation = gameObject.GetComponent<Animation>();
            string deathClip = animation.clip.name.Split('|')[0] + "|Death";
            animation.Play(deathClip);
            Destroy(gameObject, animation[deathClip].length);

            //TODO Find better place for this
            if (gameObject.name.Contains("prefab_unit_Wolpetinger"))
            {
                attacker.transform.root.GetComponent<Resources>().AddSnowballs(100); //TODO Make snowball amount dynamic
                SpawnWolpetingers spawnCtrl = transform.root.GetComponent<SpawnWolpetingers>();
                ++(spawnCtrl.totalKilled);
                --(spawnCtrl.numberAlive);

                if (spawnCtrl.numberAlive == 0)
                {
                    //Debug.Log("SPAWNING WOLPETINGER!!!!!");
                    spawnCtrl.numberAlive = spawnCtrl.Spawn();
                    PlayerPrefs.SetInt("WOLPETINGERDEATHCOUNT", PlayerPrefs.GetInt("WOLPETINGERDEATHCOUNT") + 1);
                }
            }
        }
    }

    /// <summary>
    /// Set health of Unit
    /// </summary>
    /// <param name="unitHealth">Unit Health</param>
    public void SetHealth(float unitHealth = 100)
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

    public void ManaRegen()
    {
        mana += 2;
    }

    public void SetMana(float unitMana = 100)
    {
        mana = unitMana;
    }

    /// <summary>
    /// Enables/Disables the unit.
    /// </summary>
    public void EnableUnit()
    {
        if (isUnitEnabled == false)
        {
            isUnitEnabled = true;
        }
        else
        {
            isUnitEnabled = false;
        }
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
