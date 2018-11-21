using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Squad Manager
/// </summary>
public class SquadManager : MonoBehaviour
{
    public const int NUMBER_OF_SQUADS = 6;
    private List<GameObject>[] squads;

    /// <summary>
    /// Initializes a new instance of the <see cref="SquadManager"/> class.
    /// </summary>
    SquadManager()
    {
        squads = new List<GameObject>[NUMBER_OF_SQUADS];
        for (int i = 0; i < NUMBER_OF_SQUADS; i++)
        {
            squads[i] = new List<GameObject>();
        }
    }

    /// <summary>
    /// Adds unit to squad.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    /// <param name="unit">Unit.</param>
    public void AddToSquad(int squadNumber, GameObject unit)
    {
        if (squads[squadNumber - 1].Contains(unit) || unit == null) return;

        squads[squadNumber - 1].Add(unit);
        //Debug.Log("Unit '" + unit.name + "' added to squad " + squadNumber + ".");
    }

    /// <summary>
    /// Adds units to squad.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    /// <param name="units">Units.</param>
    public void AddToSquad(int squadNumber, List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            AddToSquad(squadNumber, unit);
        }
    }

    /// <summary>
    /// Deletes the squad.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    public void DeleteSquad(int squadNumber)
    {
        squads[squadNumber - 1].Clear();
    }

    /// <summary>
    /// Removes unit from squad.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    /// <param name="unit">Unit.</param>
    public void RemoveFromSquad(int squadNumber, GameObject unit)
    {
        if (squads[squadNumber - 1].Contains(unit))
        {
            squads[squadNumber - 1].Remove(unit);
            //Debug.Log("Unit (" + unit.name + ") removed from squad: " + squadNumber );
        }
    }

    /// <summary>
    /// Removes units from squad.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    /// <param name="units">Units.</param>
    public void RemoveFromSquad(int squadNumber, List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            RemoveFromSquad(squadNumber, unit);
        }
    }

    /// <summary>
    /// Removes unit from all squads.
    /// </summary>
    /// <param name="unit">Unit.</param>
    public void RemoveFromAllSquads(GameObject unit)
    {
        for (int squad = 1; squad <= NUMBER_OF_SQUADS; squad++)
        {
            RemoveFromSquad(squad, unit);
        }
    }

    /// <summary>
    /// Gets the units from squad.
    /// </summary>
    /// <returns>The units from squad.</returns>
    /// <param name="squadNumber">Squad number.</param>
    public List<GameObject> GetUnitsFromSquad(int squadNumber)
    {
        return squads[squadNumber - 1].ToList<GameObject>();
    }

    /// <summary>
    /// Squad units do the action.
    /// </summary>
    /// <param name="squadNumber">Squad number.</param>
    /// <param name="action">Action.</param>
    public void SquadAction(int squadNumber, UnitCommands.Actions action)
    {
        UnitCommands.UnitAction(GetUnitsFromSquad(squadNumber), action);
    }
}
