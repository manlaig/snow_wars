using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Old Method that Has been replaced by movement using Nav Mesh Agents
/// 
/// Move.
/// Attach to Unit and set speeds to allow movement by right clicking the ground.
///
/// Known Issue:
/// Gravity not taken into account
/// Obstacles not taken into account (No pathfinding)
///
/// Assumed Preconditions:
/// Animation Numbering Assumed as follows
/// 0 Base
/// 1 Death
/// 2 Gather
/// 3 Idle
/// 4 Run
///
/// </summary>
public class Move : MonoBehaviour {
	// Movement Member Variables
	protected bool rotating;
	protected bool moving;
	private Vector3 destination;
	private Quaternion targetRotation;
	public float moveSpeed, rotateSpeed;
	private Animation anim;
	private List<string> animNames;

	// Use this for initialization
	void Start () {
		rotating = false;
		moving = false;
		anim = GetComponent<Animation>();

		animNames = new List<string> ();
		foreach(AnimationState state in anim)
		{
			animNames.Add (state.name);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1))
		{
			RaycastHit hit;
			/// Only starts new Movement sequence on valid click
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 200)) {
				StartMove(new Vector3 (hit.point.x, hit.point.y, hit.point.z));
			}
		}
		if (rotating)
		{
			TurnToTarget ();
		}else if (moving)
		{
			MakeMove ();
		}
		if (moving)
		{
			// Run Animation
			anim.CrossFade (animNames[4]);
		} else
		{
			// Idle Animation
			anim.CrossFade (animNames[3]);
		}
	}

	public void StartMove(Vector3 destination)
	{
		this.destination = destination;
		targetRotation = Quaternion.LookRotation (destination - transform.position);
		rotating = true;
		moving = false;
	}

	private void TurnToTarget()
	{
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotateSpeed);
		//sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
		Quaternion inverseTargetRotation = new Quaternion(-targetRotation.x, -targetRotation.y, -targetRotation.z, -targetRotation.w);
		if (transform.rotation == targetRotation || transform.rotation == inverseTargetRotation)
		{
			rotating = false;
			moving = true;
		}
	}

	private void MakeMove()
	{
		// Insert change animation code
		transform.position = Vector3.MoveTowards (transform.position, destination, Time.deltaTime * moveSpeed);
		if (transform.position == destination) {
			moving = false;
		}
	}
}
