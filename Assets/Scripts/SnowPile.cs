using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SnowPile Class
/// </summary>
public class SnowPile : MonoBehaviour
{
    public int pileSize;
    public int maxSnowBalls = 100;
    public int minSnowBalls = 10;
    protected bool beingMined = false;

    private bool isDepleted = false;
    private bool isRenewable = true;  // need to discuss if snowpiles are gonna be renewable or not
    private bool isRenewing = false;  // or if they're gonna respawn at a random location

    private float renewWaitTime = 300f;

    /// <summary>
    /// Gets the snow pile location.
    /// </summary>
    /// <returns>The snow pile location as Vector3 using only the x and z components.</returns>
    public Vector3 GetLocation()
    {
        return new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }

    /// <summary>
    /// Called at the start of the program
    /// <summary>
    private void Start()
    {
        pileSize = Random.Range(minSnowBalls, maxSnowBalls);
    }

    /// <summary>
    /// Called once per frame
    /// <summary>
    private void Update()
    {
        if (isDepleted && !isRenewable)
            Destroy(gameObject);

        else if (isDepleted && isRenewable && !isRenewing)
        {
            Destroy(gameObject);
            StartCoroutine(renewSnowPile());
        }
    }

    /// <summary>
    /// Renews the snowballs when the SnowPile is depleted
    /// <summary>
    private IEnumerator renewSnowPile()
    {
        isRenewing = true;
        yield return new WaitForSeconds(renewWaitTime);
        pileSize = Random.Range(minSnowBalls, maxSnowBalls);
        Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        isRenewing = false;
        isDepleted = false;
    }

    /// <summary>
    /// Called when an object collider makes contact with the SnowPile collider
    /// <summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Worker")
        {
            beingMined = true;
            pileSize -= 1;

            if (pileSize == 0)
                isDepleted = true;
        }
    }
}
