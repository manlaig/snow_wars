using UnityEngine;

/// <summary>
/// Base Hero Class
/// </summary>
public class BaseHero : Unit
{
    public enum StrategyType { Mid, Attack, Defense, Economy };
    public enum MicroAbility { NaughtyOrNice, SnowBlast, HiddenRocks };
    public enum PassiveAbility { ArmorBoost, ManaRegen, AttackBoost };

    [SerializeField]
    protected int presentProbability = 25;
    [SerializeField]
    private StrategyType st;
    [SerializeField]
    private MicroAbility ma;
    [SerializeField]
    private PassiveAbility pa;


    public void setStrategyType(StrategyType strategy)
    {
        st = strategy;
    }

    public void setMicroAbility(MicroAbility microAbility)
    {
        ma = microAbility;
    }

    public void setPassiveAbility(PassiveAbility passiveAbility)
    {
        pa = passiveAbility;
    }

    public void UseStrategyType()
    {
        switch (st)
        {
            case StrategyType.Mid:
                Debug.Log("<color=orange>Mid</color>");
                break;
            case StrategyType.Attack:
                Debug.Log("<color=yellow>Attack</color>");
                break;
            case StrategyType.Defense:
                Debug.Log("<color=blue>Defense</color>");
                break;
            case StrategyType.Economy:
                Debug.Log("<color=green>Economy</color>");
                break;
            default:
                Debug.Log("<color=red>No StrategyType Selected</color>");
                break;
        }
    }

    public void UseMicroAbility()
    {
        switch (ma)
        {
            case MicroAbility.NaughtyOrNice:
                Debug.Log("<color=orange>NaughtyOrNice</color>");
                break;
            case MicroAbility.SnowBlast:
                Debug.Log("<color=yellow>SnowBlast</color>");
                break;
            case MicroAbility.HiddenRocks:
                Debug.Log("<color=blue>HiddenRocks</color>");
                break;
            default:
                Debug.Log("<color=red>No MicroAbility Selected</color>");
                break;
        }
    }

    public void UsePassiveAbility()
    {
        switch (pa)
        {
            case PassiveAbility.ArmorBoost:
                Debug.Log("<color=orange>ArmorBoost</color>");
                break;
            case PassiveAbility.ManaRegen:
                Debug.Log("<color=yellow>ManaRegen</color>");
                break;
            case PassiveAbility.AttackBoost:
                Debug.Log("<color=blue>AttackBoost</color>");
                break;
            default:
                Debug.Log("<color=red>No PassiveAbility Selected</color>");
                break;
        }
    }
}