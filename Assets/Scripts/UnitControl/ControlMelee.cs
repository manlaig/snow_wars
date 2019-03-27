using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ControlMelee.
///
/// Control script for melee units.
/// Inherits from ControlBasic.cs
///
/// Note for maintaining this code:
/// As the only real difference between ControlRange and ControlMelee
/// is a different default attackRange, these should be merged into one script.
/// ControlRange is better maintained than this Script and for any
/// differences found between them, ControlRange has the more up to
/// date version of the code.
/// </summary>
public class ControlMelee : ControlBasic
{
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

    // Patrol Variables
    protected ArrayList patrolPoints;
    protected int patrolStage;

    // Target / Destination / Home(Spawn Point)
    protected Vector3 home;
    protected Transform target;
    protected Vector3 destination;
    protected Collider[] hitColliders;
    protected Collider closestEnemy;

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

        // Attacking Variables
        attackRange = 2.5f;

        // Attacking Variables
        attackRange = 30;

        agroRange = 50;
        farDistance = agroRange * 3;

        // Use following with proper function to get real values once getters are created
        damage = gameObject.GetComponent<Unit>().GetDamagePerHit();
        //damage = 10.0f;

        // Patrol Varibales
        patrolStage = 0;
        patrolPoints = new ArrayList();

        // Target / Destination / Home(Spawn Point)
        destination = transform.position;
        home = transform.position;
    }

    // Update is called once per frame
    new void Update()
    {
        if (unit.GetHealth() <= 0)
            return;

        // Handle Right Click (User Command)
        // team == 1 for testing purposes, replace with code to check if team is the players team
        if (transform.root.GetComponent<Player>().human && Input.GetMouseButtonDown(1) && selected)
        {
            /// Only starts new Movement sequence on valid click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo) && (hitInfo.transform != transform))
            {
                //Debug.Log (hitInfo.transform.gameObject.name);
                agent.destination = hitInfo.point;
                commanded = true;
                halted = false;

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
                    home = hitInfo.transform.position;
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
            {
                if (!commanded)
                    GetColliders();
            }
            else
            {
                if ((transform.position - home).sqrMagnitude < (agroRange * agroRange))
                {
                    reseting = false;
                }
            }

            // Reset to Home/Spawn if too far away
            if (!patrolling && (transform.position - home).sqrMagnitude > (farDistance * farDistance) || target == null)
            {
                agent.destination = home;
                target = null;
                attacking = false;
                reseting = true;
            }

            // Set Destination
            if (patrolling && !attacking)
            {
                if ((transform.position - (Vector3)patrolPoints[patrolStage]).sqrMagnitude < Mathf.Pow((agent.radius + attackRange), 2))
                {
                    //++patrolStage;
                    //if (patrolStage >= patrolPoints.Count)
                    //	patrolStage = 0;
                    patrolStage = (patrolStage + 1) % patrolPoints.Count;
                }
                agent.destination = (Vector3)patrolPoints[patrolStage];
            }
            else if (attacking)
            {
                // Attack Sequence
                if ((transform.position - target.position).sqrMagnitude < Mathf.Pow((agent.radius + attackRange), 2))
                {
                    agent.destination = transform.position;
                    commanded = false;
                    Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                    // the 10 in the following line is the rotation speed
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);

                    if (target.gameObject.GetComponent<Unit>().GetHealth() > 0)
                    {
                        StartCoroutine(AttackCoroutine());
                    }
                    else
                    {
                        target = null;
                        targetLock = false;
                    }
                    /**
                    if (!attackCoroutineRunning)
                    {
                        attackCoroutineRunning = true;
                        anim.CrossFade(animNames["Attack"]);
                        StartCoroutine(
                            target.gameObject.GetComponent<ControlBasic>().Hit(
                                                                                transform.position,
                                                                                attackTime,
                                                                                attackRecoil,
                                                                                damage,
                                                                                unit.GetAttackRecharge(),
                                                                                gameObject));
                    }*/
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
                commanded = false;

            }
        }
        else if (halted)
        {
            attacking = false;
        }

        // Set Animations
        if ((agent.velocity.magnitude) > 0.1)
        {
            // Run Animation
            anim.SetBool("Movement", true);
        }
        else
        {
            anim.SetBool("Movement", false);

            if (attacking)
            {
                // Attack
                anim.SetBool("Action", true);
                anim.SetBool("Attack", true);
            }
            else
            {
                if (!inHit)
                {
                    // Idle
                    anim.SetBool("Action", false);
                    anim.SetBool("Attack", false);
                }
            }
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
                if (unit.gameObject.GetComponent<Unit>().GetHealth() > 0 &&
                    (
                        !target
                        || !closestEnemy
                        || (transform.position - unit.ClosestPoint(transform.position)).sqrMagnitude < (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude
                    )
                    )
                /// End of if's boolean condition
                {
                    closestEnemy = unit;
                    if (!target)
                        target = closestEnemy.transform;
                }
            }
        }
        if (closestEnemy && (transform.position - closestEnemy.ClosestPoint(transform.position)).sqrMagnitude < (agroRange * agroRange))
        {
            target = closestEnemy.transform;
            attacking = true;
        }
        if (!target)
        {
            attacking = false;
            reseting = true;
        }
    }

    void MoveToLocation(Vector3 destination)
    {
        agent.destination = destination;
    }

    void SetHome(Vector3 point)
    {
        home = point;
    }

    public IEnumerator AttackCoroutine()
    {
        if (attackCoroutineRunning)
            yield break;
        attackCoroutineRunning = true;

        if (target.gameObject.GetComponent<Unit>().GetHealth() > 0)
        {
            target.gameObject.GetComponent<ControlBasic>().GetHit(damage, gameObject);
        }

        yield return new WaitForSeconds(attackTime + attackRecoil);

        attackCoroutineRunning = false;
    }

    new public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetLock = false;
    }

    new public void LockTarget(Transform newTarget)
    {
        target = newTarget;
        targetLock = true;
    }

    new public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }
}
