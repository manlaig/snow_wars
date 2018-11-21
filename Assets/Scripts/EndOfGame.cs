using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// End of game.
/// Correct Panel selected based on End of game condition.
/// </summary>
public class EndOfGame : MonoBehaviour
{
    private GameObject canvas;
    private Transform winPanel;
    private Transform losePanel;
    private Transform disconnectPanel;
    private Transform specialEventPanel;
    public Text specialEventText;


    private IEnumerator coroutine;
    void Start()
    {
        int slayCount = PlayerPrefs.GetInt("WOLPETINGERDEATHCOUNT");
        canvas = GameObject.Find("Canvas");
        winPanel = canvas.transform.Find("winPanel");
        losePanel = canvas.transform.Find("losePanel");
        disconnectPanel = canvas.transform.Find("disconnectPanel");
        specialEventPanel = canvas.transform.Find("specialEventPanel");

        /// "EndGameState"
        /// Win game	1
        /// Lose game	0
        /// Disconnect	2
        /// Wolpetinger Event 3
        if (PlayerPrefs.GetInt("EndGameState") == 1)
            winPanel.gameObject.SetActive(true);
        else if (PlayerPrefs.GetInt("EndGameState") == 2)
            disconnectPanel.gameObject.SetActive(true);
        else if (PlayerPrefs.GetInt("EndGameState") == 3)
        {
            specialEventPanel.gameObject.SetActive(true);
            specialEventText.text = "You Survied! \nYou slayed " + slayCount + " Wolpetingers!";

        }

        else
            losePanel.gameObject.SetActive(true);

        coroutine = HoldOnGameOver();
        // Replaced timer with buttons
        //StartCoroutine(coroutine);

        // Clear stats/state for new game;
        ResetPlayerStats();
    }

    IEnumerator HoldOnGameOver()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void ResetPlayerStats()
    {
        PlayerPrefs.SetFloat("EndGameState", 0f);
        PlayerPrefs.SetFloat("WOLPETINGERDEATHCOUNT", 0f);
    }
}
