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
    protected UnityEngine.AI.NavMeshAgent agent;
    protected RaycastHit hitInfo = new RaycastHit();
    protected Animation anim;
    protected bool isHit = false;
    protected bool inHit = false;
    protected Dictionary<string, string> animNames;
    public bool selected = false;
    public bool halted;
    public int team;
    protected Unit unit;
    public bool attackCoroutineRunning = false;

    [SerializeField]
    protected GameObject selectionArrow;

    // Use this for initialization
    protected void Start()
    {
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Initialize Animation Control
        anim = GetComponent<Animation>();
        animNames = new Dictionary<string, string>();
        foreach (AnimationState state in anim)
        {
            string stripped = state.name.Substring(state.name.IndexOf("|") + 1);
            animNames.Add(stripped, state.name);
        }

        unit = gameObject.GetComponent<Unit>();
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

    // Update is called once per frame
    protected void Update()
    {
        /// Implement Left click to toggle selected when ray hits self
        /*
		if (Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray.origin, ray.direction, out hitInfo))
			{
				// team == 1 for testing purposes, replace with code to check if team is the players team
				if (transform.root.GetComponent<Player>().human && hitInfo.transform.position == transform.position)
					toggleSelected ();
				else
					selected = false;
			}
		}
        // */
    }

    // Control Animation when hit by attacker
    public void Hit(Vector3 attackerPosition, float attackTime, float attackRecoil, float damage, float recharge, GameObject _attacker)
    {
        /*
        ControlBasic attacker = _attacker.GetComponent<ControlBasic>();

		// Turn towards attacker
		Quaternion targetRotation = Quaternion.LookRotation (attackerPosition - transform.position);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, 10);

		// Delay Hit animation to synchronize with attack animation
		if (isHit)
			yield break;
		isHit = true;
		yield return new WaitForSeconds (attackTime);

        // Perform Hit animation
        try
        {

            inHit = true;
            if (animNames.ContainsKey("Hit"))
            {
                anim.CrossFade(animNames["Hit"]);
                if (agent.velocity.magnitude > 0.1)
                    anim.CrossFadeQueued(animNames["Run"]);
                else
                    anim.CrossFadeQueued(animNames["Idle"]);
            }
       	*/
        // Take Damage
        gameObject.GetComponent<Unit>().TakeDamage(_attacker, damage);
        /*
        }
        catch (Exception e)
        {
            Debug.Log("DEBUG_ERROR:" + e.Message);
        }
		//yield return new WaitForSeconds (0);

        // Delay to synchronize attack cycle
        yield return new WaitForSeconds (attackRecoil-attackTime);

		// Reset to resume normal behavior
		inHit = false;
		isHit = false;
        // Temporary

        yield return new WaitForSeconds(recharge);

        attacker.attackCoroutineRunning = false;
		*/
    }

    public void toggleSelected()
    {
        selected = !(selected);
    }

    public void HaltUnit()
    {
        halted = true;
    }
}
