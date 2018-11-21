using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core system.
/// </summary>
public class CoreSystem : MonoBehaviour
{
    public enum Player { One = 0, Two };
    public const int NUMBER_OF_PLAYERS = 2;

    public enum TAGS_OFFENSIVE
    {
        Piercing,
        Venom,
        Crippling,
        Blunt,
        Melee,
        Snow,
        Ranged,
        Grumpy,
        Vampiric,
        Siege,
        Splash
    };

    public enum TAGS_DEFENSE
    {
        X_Immunity,
        X_Resist,
        X_Vulnerability,
        X_Weakness,
        Structure,
        Biological,
        Flying,
        Ground
    };

    public enum TAGS_CONDITIONS
    {
        Stunned,
        Frightened,
        Frozen,
        Feeble,
        Mighty,
        Sluggish,
        Frenzied,
        Jolly,
        Courageous,
        Stacking,
        InCombat,
        OutofCombat,
        Crippled,
        Envenomed
    };

    [SerializeField]
    private bool DEBUG_MODE = true;
    private TeamInfo[] players;

    struct TeamInfo
    {
        public bool origMainTreeAlive;
        public bool origMainCastleStanding;
        public Color teamColor;

        public TeamInfo(string anything)
        {
            origMainTreeAlive = true;
            origMainCastleStanding = true;
            teamColor = Color.magenta;
        }
    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        players = new TeamInfo[NUMBER_OF_PLAYERS];
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            players[i] = new TeamInfo("new");
        }
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening(EventManager.Events.TreeDead, setMainTreeDead);
        EventManager.StartListening(EventManager.Events.CastleDistroyed, setMainCastleDistroyed);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening(EventManager.Events.TreeDead, setMainTreeDead);
        EventManager.StopListening(EventManager.Events.CastleDistroyed, setMainCastleDistroyed);
    }

    /// <summary>
    /// Gets the color of the team.
    /// </summary>
    /// <returns>The team color.</returns>
    /// <param name="p">P.</param>
    public Color GetTeamColor(Player p)
    {
        return players[(int)p].teamColor;
    }

    /// <summary>
    /// Sets the color of the team.
    /// </summary>
    /// <param name="p">P.</param>
    /// <param name="c">C.</param>
    public void SetTeamColor(Player p, Color c)
    {
        players[(int)p].teamColor = c;
    }

    /// <summary>
    /// Sets the DEBUGMOD.
    /// </summary>
    /// <param name="_DEBUG_MODE">If set to <c>true</c> DEBU g MOD.</param>
    public void setDEBUGMODE(bool _DEBUG_MODE = false)
    {
        DEBUG_MODE = _DEBUG_MODE;
    }

    /// <summary>
    /// Sets the main tree dead.
    /// </summary>
    /// <param name="go">Go.</param>
    public void setMainTreeDead(GameObject go)
    {
        //Player p = new Player();
        //p = go.GetComponent<Tree>().GetPlayer(); //Where is player (enum) stored per player?
        //setMainTreeDead(p);
    }

    /// <summary>
    /// Sets the christmas tree dead.
    /// </summary>
    public void setMainTreeDead(Player p)
    {
        players[(int)p].origMainTreeAlive = false;
        CheckForWin();
    }

    /// <summary>
    /// Sets the main castle destroyed.
    /// </summary>
    /// <param name="go">Go.</param>
    void setMainCastleDistroyed(GameObject go)
    {
        //Player p = new Player();
        //p = go.GetComponent<BaseBuilding>().GetPlayer(); //Where is player (enum) stored per player?
        //setMainCastleDistroyed(p);
    }

    /// <summary>
    /// Sets the snow castle dead.
    /// </summary>
    public void setMainCastleDistroyed(Player p)
    {
        //snowCastleLive = false;
        CheckForWin();
    }

    /// <summary>
    /// Checks for win.
    /// </summary>
    void CheckForWin()
    {
        if (DEBUG_MODE == true)
        {
            //christmasTreeLive = false;
            //snowCastleLive = false;
        }

        if (players[(int)Player.One].origMainTreeAlive == false &&
            players[(int)Player.One].origMainCastleStanding == false)
        {
            /// "EndGameState"
            /// Win game	1
            /// Lose game	0
            /// Disconnect	2
            /// Wolpetinger Event 3
            PlayerPrefs.SetInt("EndGameState", 0);
            SceneManager.LoadScene("EndOfGame", LoadSceneMode.Single);
        }
    }
}
