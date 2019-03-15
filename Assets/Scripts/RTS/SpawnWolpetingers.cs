/*
Spawn Wolpetingers.

Used for special Wolpetinger demo
 */

using UnityEngine;

public class SpawnWolpetingers : MonoBehaviour
{
    public GameObject Wopletinger;

    // Used for repeated spawning
    //public float spawnTime = 2f;

    public Transform spawnPoint;
    public static int wave = 0;
    public float spawnVariance = 15;

    public int numberAlive = 1;

    void Start()
    {
        Spawn(10);
    }

    /// <summary>
    /// Spawn for Wave
    /// </summary>
    public int Spawn()
	{
		// How many to spawn in this wave
		int amount = (wave / 2) + 1;
		Debug.Log ("prespawn # Wol: " + numberAlive.ToString ());
		for (int i = 0; i < amount; ++i)
		{
            // Spawn with random variance in position
            SpawnAtSpawnPoint();
		}
		// Update the amount of wolpetingers that are alive
		numberAlive += amount;
		Debug.Log ("wave: " + wave.ToString ());
		Debug.Log ("# Wol: " + numberAlive.ToString ());
		++wave;
		return amount;
	}

	/// <summary>
	/// Spawn specific amount
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void Spawn(int amount)
	{
		for (int i = 0; i < amount; ++i)
		{
            // Spawn with random variance in position
            SpawnAtSpawnPoint();
		}
		// Update the amount of wolpetingers that are alive
		numberAlive += amount;
	}

    void SpawnAtSpawnPoint()
    {
        Instantiate(Wopletinger, (spawnPoint.position + new Vector3(spawnVariance * (Random.value - 0.5f), 0, spawnVariance * (Random.value - 0.5f))), spawnPoint.rotation);
    }
}
