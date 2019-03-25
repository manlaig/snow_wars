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
    protected Animation anim;
    protected bool isHit = false;
    protected bool inHit = false;
    protected Dictionary<string, string> animNames;
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
        anim = GetComponent<Animation>();
        animNames = new Dictionary<string, string>();

        foreach (AnimationState state in anim)
        {
            /*
             * Animations are formatted in a way, for example: Worker|Idle, Santa|Hit
             * The line below is getting the title of the animation, like Idle and Hit
             */
            string stripped = state.name.Substring(state.name.IndexOf("|") + 1);
            animNames.Add(stripped, state.name);
        }
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
