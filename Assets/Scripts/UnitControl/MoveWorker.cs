using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Move worker.
///
/// Control script for workers.
/// Inherits from ControlBasic.cs
/// </summary>
public class MoveWorker : ControlBasic
{
    protected bool gathering;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        gathering = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(1) && selected)
        {
            /// Only starts new Movement sequence on valid click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo) && (hitInfo.transform != transform))
            {
                //Debug.Log (hitInfo.transform.gameObject.name);
                agent.destination = hitInfo.point;
                if (hitInfo.transform.gameObject.name.Contains("SnowMound"))
                    gathering = true;
                else
                    gathering = false;
            }
        }

        if (agent.velocity.magnitude > 0.1)
        {
            // Run Animation
            anim.CrossFade(animNames["Run"]);
        }
        else
        {
            if (gathering)
            {
                // Gather
                anim.CrossFade(animNames["Gather"]);
            }
            else
            {
                if (!inHit)
                {
                    // Idle Animation
                    anim.CrossFade(animNames["Idle"]);
                }
            }
        }
    }
}
