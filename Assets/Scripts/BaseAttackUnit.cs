using UnityEngine;

/// <summary>
/// BaseAttackUnit class
/// <summary>
public class BaseAttackUnit : BaseWorker
{
    public enum MicroAbility
    {
        PeppermintEnhance, SugarHigh, Sneak,
        /*Missing JockElf*/ /*Missing NerdElf*/
    };

    /// <summary>
    /// Calls Unit's MicroAbility
    /// </summary>
    public void UseMicroAbility(MicroAbility ma)
    {
        switch ((int)ma)
        {
            case 1:
                Debug.Log("<color=orange>PeppermintEnhance</color>");
                break;
            case 2:
                Debug.Log("<color=yellow>SugarHigh</color>");
                break;
            case 3:
                Debug.Log("<color=blue>Sneak</color>");
                break;
            case 4:
                //Debug.Log("<color=green>Missing JockElf</color>");
                break;
            case 5:
                //Debug.Log("<color=violet>/*Missing NerdElf*/</color>");
                break;
            default:
                Debug.Log("<color=red>No MicroAbility Selected</color>");
                break;
        }
    }
}
