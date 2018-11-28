using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ControlMelee.
///
/// Control script for Wopletinger unit.
///
/// Certain default values have been changed for desired demo purposes.
/// At a later date Wopletingers should be able to use the same script
/// as other attackers. The prefab or units just need those values set
/// correctly in the inspector.
/// </summary>
public class ControlWopletinger : ControlBasic
{
    // Action States
    private bool attacking;
    private bool patrolling;
    private bool reseting;

    // Attacking Variables
    protected float attackRange;
    protected float attackTime;
    protected float attackRecoil;
    protected float agroRange;
    protected float farDistance;
    protected float damage;

    // Patrol Variables
    private ArrayList patrolPoints;
    private int patrolStage;

    // Target / Destination / Home (Spawn Point)
    private Vector3 home;
    private Transform target;
    private Vector3 destination;
    private Collider[] hitColliders;
    private Collider closestEnemy;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        // Action States
        attacking = false;
        patrolling = false;
        reseting = false;
        halted = false;

        // Attacking Variables
        attackRange = 4f;

		// Times(float) used to synchronize attack animations
		attackTime = gameObject.GetComponent<Unit>().GetHitDelay();
        attackRecoil = gameObject.GetComponent<Animation>()[animNames["Attack"]].length;

        agroRange = 1000f;
        farDistance = agroRange * 3;

		// Use following with proper function to get real values once getters are created
		damage = gameObject.GetComponent<Unit>().GetDamagePerHit();

		// Patrol Variables (part of old patrol system)
		patrolStage = 0;
		patrolPoints = new ArrayList ();

        // Target / Destination / Home(Spawn Point)
        destination = transform.position;
        home = transform.position;
    }

	// Update is called once per frame
	new void Update ()
    {
		// If already DEAD (HP <= 0)
		// Then do not continue
        if (unit.GetHealth() <= 0)
            return;

        // Handle Right Click (User Command)
        // team == 1 for testing purposes, replace with code to check if team is the players team
        //transform.root.GetComponent<Player>()
        if (Input.GetMouseButtonDown(1) && selected)
        {
            /// Only starts new Movement sequence on valid click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo) && (hitInfo.transform != transform))
            {
                //Debug.Log (hitInfo.transform.gameObject.name);
                agent.destination = hitInfo.point;

                /// Change if for who should be attackable
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    //Debug.Log ("Patrol Point");
                    patrolPoints.Add(hitInfo.point);
                    if (patrolPoints.Count >= 2)
                    {
                        patrolling = true;
                    }
                }
                else if (hitInfo.transform.gameObject.GetComponent<ControlBasic>() && hitInfo.transform.root != transform.root)
                {
                    attacking = true;
                    target = hitInfo.transform;
                    // Reset Patrolling
                    if (patrolPoints.Count >= 2)
                    {
                        patrolPoints.Clear();
                        patrolStage = 0;
                        patrolling = false;
                    }
                }
                else
                {
                    destination = hitInfo.point;
                    attacking = false;
                    home = destination;
                    // Reset Patrolling
                    if (patrolPoints.Count >= 2)
                    {
                        patrolPoints.Clear();
                        patrolStage = 0;
                        patrolling = false;
                    }
                }
            }
        }

        if (!halted)
        {
            // Low Level AI
            // Find Enemy Target
            // Look for enemies inside of agro range.
            if (!reseting)
                GetColliders();
            else
            {
                if ((transform.position - home).sqrMagnitude < (agroRange * agroRange))
                {
                    reseting = false;
                    attacking = false;
                }
            }

            // Reset to Home/Spawn if too far away
            if (!patrolling && (transform.position - home).sqrMagnitude > (farDistance * farDistance) || (target == null && attacking))
            {
                agent.destination = home;
                target = null;
                attacking = false;
                reseting = true;
            }

            // Set Destination
            if (patrolling && !attacking)
            {
                if ((transform.position - (Vector3)patrolPoints[patrolStage]).sqrMagnitude < Mathf.Pow(attackRange, 2))
                {
                    patrolStage = (patrolStage + 1) % patrolPoints.Count;
                }
                agent.destination = (Vector3)patrolPoints[patrolStage];
            }
            else if (attacking)
            {
                // Attack Sequence
                if ((transform.position - target.position).sqrMagnitude < Mathf.Pow(attackRange, 2))
                {
                    agent.destination = transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                    // the 10 in the following line is the rotation speed
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);

                    StartCoroutine(AttackCoroutine());
                }
                else
                {
                    // Chase Enemy
                    agent.destination = target.position;
                }
            }
            else if ((transform.position - destination).sqrMagnitude < (agent.radius * agent.radius + 1))
            {
                // Stop when Destination reached
                agent.destination = transform.position;
            }
        }

        try
        {
            // Set Animations
            if ((agent.velocity.magnitude) > 0.1)
            {
                // Run Animation
                anim.CrossFade(animNames["Run"]);
            }
            else
            {
                if (attacking)
                {
                    // Attack
                    anim.CrossFade(animNames["Attack"]);
                }
                else
                {
                    if (!inHit)
                    {
                        // Idle
                        anim.CrossFade(animNames["Idle"]);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("DEBUG_ERROR:" + e.Message);
        }
    }

    // Look for enemies inside of agro range.
    void GetColliders()
    {
        hitColliders = Physics.OverlapSphere(transform.position, agroRange);
        foreach (Collider unit in hitColliders)
        {
            if (unit.gameObject.GetComponent<ControlBasic>() && unit.transform.root != transform.root)
            {
                if (!closestEnemy || (transform.position - unit.ClosestPoint(transform.position)).sqrMagnitude < (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude)
                {
                    closestEnemy = unit;
                }
            }
        }
        if (closestEnemy && (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude < (agroRange * agroRange))
        {
            target = closestEnemy.transform;
            attacking = true;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        if (attackCoroutineRunning)
            yield break;
        attackCoroutineRunning = true;

        yield return new WaitForSeconds(attackTime);

        if (target == null)
            yield break;

        target.gameObject.GetComponent<ControlBasic>().GetHit(damage, gameObject);

        yield return new WaitForSeconds(attackRecoil - attackTime);

        attackCoroutineRunning = false;
    }
}
