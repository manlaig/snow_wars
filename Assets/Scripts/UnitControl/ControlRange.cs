using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


/*
 * This script needs a change
 * 1. It is spawning a selection arrow depending on how many units are selected.
 *    In other words, if you select 4 units and right click, 4 selection arrows are spawned at the same position
 * 2. Use setting active/inactive instead of instantiating and destroying
 */

/*
 * Important points:
 * 1. The way this script is designed so that all units under the control of the player MUST be a child gameObject of it.
 *    If they don't share the same root gameobject, then they are considered enemies. This script can be optimized a lot.
 */



/// <summary>
/// ControlRange.
///
/// Control script for ranged units.
/// Inherits from ControlBasic.cs
/// </summary>
public class ControlRange : ControlBasic
{
    [SerializeField] LayerMask layerMask;
	// Action States
	[SerializeField]
	protected bool attacking;
	protected bool patrolling;
	[SerializeField]
	protected bool reseting;
	protected bool commanded;
	protected bool targetLock;

	// Attacking Variables
	[SerializeField]
	protected float attackRange;
	protected float attackTime;
	protected float attackRecoil;
	protected float agroRange;
	protected float farDistance;
	protected float damage;

    [SerializeField] Transform snowballSpawn;

	// Patrol Variables
	//private ArrayList patrolPoints;
	//private int patrolStage;

	// Target / Destination / Home(Spawn Point)
	protected Vector3 home;
	protected Transform target;
	protected Vector3 destination;
	protected Collider[] hitColliders;
	protected Collider closestEnemy;

    /*
     * When the user selects units and right clicks, it spawns an arrow at the destination point
     * This is a reference to that instance of selection arrow
     */
    private GameObject arrowInstance;

	// Use this for initialization
	new void Start()
	{
		base.Start();
        // Initialize Variables

        // Action States
        attacking = false;
		patrolling = false;
		reseting = false;
		halted = false;
		commanded = false;
		targetLock = false;

		// Times(float) used to synchronize attack animations
		attackTime = GetComponent<Unit>().GetHitDelay();
		attackRecoil = GetComponent<Animation>()[animNames["Attack"]].length;

		// Hard coded Agro Settings
		agroRange = 50;
		farDistance = agroRange * 2;

		// Use following with proper function to get real values once getters are created
		damage = GetComponent<Unit>().GetDamagePerHit();

		// Patrol Varibales
		// (Used by old Patrol System, may get removed when new patrol system is implemented)
		//patrolStage = 0;
		//patrolPoints = new ArrayList();

		// Initialize Destination and Home
		// Target / Destination / Home(Spawn Point)
		destination = transform.position;
		home = transform.position;
	}

	// Update is called once per frame
	new void Update()
	{
		// If home was not initialized properly or lost then reinitialize home
		if(home == null)
			home = transform.position;

		// If already DEAD (HP <= 0)
		// Then do not continue
		if (unit.GetHealth() <= 0)
		{
			// Clean up Destination arrows if DEAD
			if (arrowInstance)
				Destroy(arrowInstance);
			return;
		}

        /*
		3 Phases of Basic AI
			1 User Command
			2 React to States
			3 Set Appropriate Animation
		 */

        if (!transform.root.GetComponent<Player>()) return;

		// 1 User Command
		// Handle User Command (Right Click)
		if (transform.root.GetComponent<Player>().human && Input.GetMouseButtonDown(1) && selected)
		{
			// Player Command, end halt and reset states
			commanded = true;
			halted = false;
			reseting = false;

			// Remove previous Destination Arrow if it exists
			if (arrowInstance)
				Destroy(arrowInstance);

			/// Only starts new Movement sequence on valid click
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, Mathf.Infinity, layerMask) && (hitInfo.transform != transform))
			{
				agent.destination = hitInfo.point;
				// Right Clicked on Attackable unit from a different player
				// Attack Command
				if (hitInfo.transform.gameObject.GetComponent<ControlBasic>() && hitInfo.transform.root != transform.root)
				{
                    Debug.Log("attacking");
					attacking = true;
					target = hitInfo.transform;
					home = hitInfo.transform.position;
				}
				else // Move Command
				{
                    //Debug.Log("moving");
                    destination = hitInfo.point;
					// Create Destination Arrow for Move Command
					arrowInstance = Instantiate(selectionArrow, transform.root.transform);
					arrowInstance.transform.position = destination;
					attacking = false;
					home = destination;
				}
                /*
                // Reset Patrolling (Part of old patrol system)
                if (patrolPoints.Count >= 2)
                {
                    patrolPoints.Clear();
                    patrolStage = 0;
                    patrolling = false;
                }
                */
            }
		}

		// Remove Destination Arrow if close enough to destination
		if (arrowInstance && (transform.position - arrowInstance.transform.position).sqrMagnitude < (agent.radius * agent.radius * 4 + 5))
		{
			// Allows Arrow to exist briefly for a very short move command
			Destroy(arrowInstance, 0.2f);
			arrowInstance = null;
		}

		// 2 React to States
		// Low Level AI
		if (!halted)
		{
			if (!reseting)
			{
				// Find Enemy Target
				// Look for enemies inside of agro range.
				// Look for enemies if (not reseting) and (not commanded) and (no targetLock)
				if (!commanded && !targetLock)
					GetColliders();
			}
			else
			{
				// Stop reseting when close to home
				if ((transform.position - home).sqrMagnitude < (agroRange * agroRange))
				{
					reseting = false;
				}
			}

			// Reset to Home/Spawn if too far away
			// 	or if target has died and a new one has not been found
			if (!patrolling && (transform.position - home).sqrMagnitude > (farDistance * farDistance) || (target == null && attacking))
			{
				agent.destination = home;
				target = null;
				attacking = false;
				reseting = true;
			}

			// Set Destination
			/*if (patrolling && !attacking)
			{
				// (part of old patrol system)
				if ((transform.position - (Vector3)patrolPoints[patrolStage]).sqrMagnitude < Mathf.Pow((agent.radius + attackRange), 2))
				{
					patrolStage = (patrolStage + 1) % patrolPoints.Count;
				}
				agent.destination = (Vector3)patrolPoints[patrolStage];
			}
			else*/ if (attacking)
			{
				// Attack Sequence
				// If in attack range attack, else Move to / Chase Enemy
				if ((transform.position - target.position).sqrMagnitude < Mathf.Pow((agent.radius + attackRange), 2))
				{
					agent.destination = transform.position;
					// Once enemy is reached, Low Level AI takes control over user command
					commanded = false;
					// Look towards enemy to attack
					Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
					// the 10 in the following line is the rotation speed
					transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);

					// Attack if alive
					if (target.gameObject.GetComponent<Unit>().GetHealth() > 0)
					{
                        StartCoroutine(AttackCoroutine());
					}
					else
					{
						// Remove target and targetLock on enemy target death
						target = null;
						targetLock = false;
					}
				}
				else
				{
					// Chase Enemy
					agent.destination = target.position;
				}
			}
			// Move Command Destination Reached
			else if ((transform.position - destination).sqrMagnitude < (agent.radius * agent.radius + 1))
			{
				// Stop when Destination reached
				agent.destination = transform.position;
				// Move Command Completed
				commanded = false;
			}
		}
		else if (halted)
		{
			// Halt stops attack state
			attacking = false;
		}

		// 3 Set Appropriate Animation
		// Set Animations
		if ((agent.velocity.magnitude) > 0.1 && !attacking)
		{
			// Run Animation
			anim.CrossFade(animNames["Run"]);
		}
		else
		{
			if (!inHit && !attacking)
			{
				// Idle
				anim.CrossFade(animNames["Idle"]);
			}
		}
	}

	// Look for enemies inside of agro range.
	void GetColliders()
	{
		// Check for enemies within Sphere of radius agroRange
		hitColliders = Physics.OverlapSphere(transform.position, agroRange);
		foreach (Collider unit in hitColliders)
		{
			// If attackable unit is owned by another player
			if (unit.gameObject.GetComponent<ControlBasic>() && unit.transform.root != transform.root)
			{
				// If it is alive, not the current target, and closer than previous target; Then it is the new target
				if (unit.gameObject.GetComponent<Unit>().GetHealth() > 0 &&
					(!target || !closestEnemy ||
					(transform.position - unit.ClosestPoint(transform.position)).sqrMagnitude < (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude)
				)
				/// End of if's boolean condition
				{
					closestEnemy = unit;
					if (!target)
						target = closestEnemy.transform;
				}
			}
		}
		// If new target(closestEnemy) is in range, Then attack it
		if (closestEnemy && (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude < (agroRange * agroRange))
		{
			target = closestEnemy.transform;
			attacking = true;
		}
		// If no target has been found, Reset to home position
		if (!target)
		{
			attacking = false;
			if((transform.position - home).sqrMagnitude > (agroRange * agroRange))
				reseting = true;
		}
	}

	// Attacking Enemy Unit in syncronized interval
	protected IEnumerator AttackCoroutine()
	{
        if (attackCoroutineRunning)
            yield break;
        else
            anim.CrossFade(animNames["Attack"], 0f);
        attackCoroutineRunning = true;

        yield return new WaitForSeconds(attackRecoil * 0.6f);

        // triggering event
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.AddComponent<Rigidbody>();
        go.transform.position = snowballSpawn.position;
        go.GetComponent<Rigidbody>().useGravity = false;
        go.GetComponent<Rigidbody>().velocity = new Vector3(target.position.x - snowballSpawn.position.x, target.position.y - snowballSpawn.position.y, target.position.z - snowballSpawn.position.z);

        yield return new WaitForSeconds(attackRecoil * 0.4f + attackTime);

        attackCoroutineRunning = false;
        /*

		// Time attacks with Attack Animation
		yield return new WaitForSeconds(attackTime + attackRecoil);
		
        // Hit target Enemy if it is alive
		if (target.gameObject.GetComponent<Unit>().GetHealth() > 0)
		{
			target.gameObject.GetComponent<ControlBasic>().GetHit(damage, gameObject);
		}
        */
    }
}
