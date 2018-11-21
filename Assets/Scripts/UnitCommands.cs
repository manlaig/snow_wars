using System.Collections.Generic;
using UnityEngine;

public static class UnitCommands
{
    public enum Actions
    {
        Attack,
        AttackDestination,
        AttackLock,
        Halt,
        Patrol,
        MicroAbility
    }

    /// <summary>
    /// Units do the action.
    /// </summary>
    /// <param name="units">Units.</param>
    /// <param name="action">Action.</param>
    public static void UnitAction(List<GameObject> units, Actions action, Transform target = null, Vector3 destination = default(Vector3))
    {
        ControlBasic control;

        foreach (GameObject unit in units)
        {
            control = unit.GetComponent<ControlBasic>();

            switch (action)
            {
                case Actions.Attack:
                    control.SetTarget(target);
                    break;
                case Actions.AttackLock:
                    control.LockTarget(target);
                    break;
                case Actions.AttackDestination:
                    control.SetDestination(destination);
                    break;
                case Actions.Halt:
                    control.HaltUnit();
                    break;
                case Actions.MicroAbility:
                    control.MicroAbility();
                    break;
                case Actions.Patrol:
                    control.Patrol();
                    break;
                default:
                    break;
            }
        }
    }
}
