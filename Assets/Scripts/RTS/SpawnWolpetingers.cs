/*
Spawn Wolpetingers.

Used for special Wolpetinger demo
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWolpetingers : MonoBehaviour
{
    public GameObject Wopletinger;

    // Used for repeated spawning
    //public float spawnTime = 2f;

    public Vector3 spawnPoint;
    public Quaternion spawnRotation;
    public static int wave = 0;
    public float spawnVariance = 15;

    public int numberAlive = 1;
    public int totalKilled = 0;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
	}

	/// <summary>
	/// Spawn for Wave
	/// </summary>
	public int Spawn()
	{
		GameObject spawn;
		// How many to spawn in this wave
		int amount = (wave / 2) + 1;
		Debug.Log ("prespawn # Wol: " + numberAlive.ToString ());
		for (int i = 0; i < amount; ++i)
		{
			// Spawn with random variance in position
			spawn = Instantiate (Wopletinger, (spawnPoint + new Vector3(spawnVariance * (Random.value - 0.5f), 0, spawnVariance * (Random.value - 0.5f))), spawnRotation);
			spawn.transform.parent = transform;
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
		GameObject spawn;
		for (int i = 0; i < amount; ++i)
		{
			// Spawn with random variance in position
			spawn = Instantiate (Wopletinger, (spawnPoint + new Vector3(spawnVariance * (Random.value - 0.5f), 0, spawnVariance * (Random.value - 0.5f))), spawnRotation);
			spawn.transform.parent = transform;
		}
		// Update the amount of wolpetingers that are alive
		numberAlive += amount;
	}
}
