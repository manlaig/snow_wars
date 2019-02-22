using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core system.
/// </summary>
public class CoreSystem : MonoBehaviour
{
    /*
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
    /// Sets the christmas tree dead.
    /// </summary>
    public void setMainTreeDead(Player p)
    {
        players[(int)p].origMainTreeAlive = false;
        CheckForWin();
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
    */
}
