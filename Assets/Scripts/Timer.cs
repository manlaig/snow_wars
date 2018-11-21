using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Game timer for Wave Survival 
/// </summary>
public class Timer : MonoBehaviour
{
    public float timeLeftMin = 7.0f;
    public float timeLeftSec = 0.0f;
    public Text timerText;

    // Use this for initialization
    void Start()
    {

        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        timerText.text = timeLeftMin.ToString() + "m " + timeLeftSec.ToString() + "s";
        yield return new WaitForSeconds(1.0f);
        while (timeLeftMin >= 0 && timeLeftSec >= 0)
        {
            while (timeLeftSec > 0)
            {
                timerText.text = timeLeftMin.ToString() + "m " + timeLeftSec.ToString() + "s";
                yield return new WaitForSeconds(1.0f);
                timeLeftSec--;
            }
            timeLeftSec = 59.0f;
            timeLeftMin--;
            if (timeLeftMin == 0 && timeLeftSec == 0)
            {
                timerText.text = "0m 0s";
            }
            else
            {
                timerText.text = timeLeftMin.ToString() + "m " + timeLeftSec.ToString() + "s";
                yield return new WaitForSeconds(1.0f);
            }


        }

        /// "EndGameState"
        /// Win game	1
        /// Lose game	0
        /// Disconnect	2
        /// Wolpetinger Event 3

        timerText.text = "Times Up";
        yield return new WaitForSeconds(1.0f);
        PlayerPrefs.SetInt("EndGameState", 3);
        SceneManager.LoadScene("EndOfGame", LoadSceneMode.Single);
    }
}
