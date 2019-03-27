using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Control basic.
///
/// Base Control script for attackable Units
/// </summary>
public class ControlBasic : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected RaycastHit hitInfo = new RaycastHit();
    protected Animator anim;
    protected bool isHit = false;
    protected bool inHit = false;
    protected bool halted;
    protected Unit unit;
    protected bool attackCoroutineRunning = false;
    public bool selected = false;

    [SerializeField]
    protected GameObject selectionArrow;

    protected void Start()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void SetDestination(Vector3 destination)
    {
        throw new NotImplementedException();
    }

    public void LockTarget(Transform target)
    {
        throw new NotImplementedException();
    }

    public void MicroAbility()
    {
        throw new NotImplementedException();
    }

    public void Patrol()
    {
        throw new NotImplementedException();
    }

    public void SetTarget(Transform target)
    {
        throw new NotImplementedException();
    }

    // Control Animation when hit by attacker
    public void GetHit(float damage, GameObject _attacker)
    {
        unit.TakeDamage(_attacker, damage);
    }

    public void toggleSelected()
    {
        selected = !selected;
    }

    public void HaltUnit()
    {
        halted = true;
    }
}
