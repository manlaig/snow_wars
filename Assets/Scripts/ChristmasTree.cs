using UnityEngine;

/// <summary>
/// Base Tree class
/// <summary>
public class ChristmasTree : BaseBuilding
{
    public enum Hero { Santa, SnowQueen, Grunch };

    public GameObject topper;

    public Hero topperType;
    [SerializeField]
    private int topperCost;
    [SerializeField]
    private Transform spawnLocation;

    //protected GameObject coreObject;

    private float topOfTree;

    ChristmasTree()
    {
        workerCapacity = 0;
    }

    /// <summary>
    /// Called at start of game
    /// <summary>
    new void Start()
    {
        base.Start();
        Instantiate(gameObject, spawnLocation.position, spawnLocation.rotation); // ?????
    }

    /// <summary>
    /// Called once per frame for physics related functions
    /// <summary>
    void FixedUpdate()
    {
        //SpawnPresents(topperType); /// need to determine which conditions award presents
    }

    public void BuyTopper(Hero topperType)
    {
        if (R.GetSnowballCount() >= topperCost)
        {
            R.RemoveSnowballs(topperCost);
            AddTopper(topperType);
        }
        else
        {
            // Insufficient Snowballs
            Debug.Log("Insufficient Snowballs");
        }
    }

    private void AddTopper(Hero _topperType)
    {
        topperType = _topperType;
    }

    /// <summary>
    /// Takes the damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    new public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            EventManager.TriggerEvent(EventManager.Events.TreeDead, gameObject);
            Destroy(gameObject);
            //Destroy(topper);
        }
    }

    /// <summary>
    /// Determines the hero based on chosen ornament and spawns at top of tree
    /// <summary>
    void ChooseTopper()
    {
        /*
    switch (topperType)
    {
      case "Santa":
			//topper texture will be changed to the Santa design;
      case "SnowQueen":
			//topper texture will be changed to the SnowQueen design;
      case "Grunch":
			//topper texture will be changed to the Grunch design;
    }
    //Instantiate(topper, spawnLocation.position.y + topOfTree,
      //spawnLocation.rotation);
    */
    }

    /// <summary>
    /// Instantiates awarded presents based on conditions of chosen hero
    /// <summary>
    void SpawnPresents(Hero hero)
    {
        /*
    switch (topperType)
    {
      case "Santa":
        /// conditions for presents
        /// chance to gain presents whenever you make a unit
      case "SnowQueen":
        /// conditions for presents
        /// chance to gain presents whenever you finish a building
      case "Grunch":
        /// conditions for presents
        /// chance to gain presents whenever you kill an enemy
    } */
    }

    public override void OnClick()
    {
        
    }

    public override void OnMouseHover()
    {

    }
}
