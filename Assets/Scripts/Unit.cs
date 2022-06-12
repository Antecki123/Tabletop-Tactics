using System;
using UnityEngine;

[RequireComponent(typeof(UnitAnimation)), SelectionBase]
public class Unit : MonoBehaviour
{
    public static Action<Unit> OnGetDamage;
    public static Action<Unit> OnDeath;

    public enum Player { Player1, Player2 , AI}

    [Header("Component References")]
    [SerializeField] private UnitStats unitBaseStats;

    [Header("Unit Properties")]
    [SerializeField] private Player unitOwner;
    [SerializeField] private Wargear wargear;
    
    [Header("Unit Statistics")]
    private int unitMove;
    private int unitSpeed;

    private int unitFightSkill;
    private int unitArcherySkill;
    private int unitStrength;
    private int unitDefence;

    private int unitActions;
    private int unitWounds;
    private int unitCourage;

    private int unitWill;
    private int unitMight;

    #region PROPERTIES
    public UnitStats UnitBaseStats { get => unitBaseStats; set => unitBaseStats = value; }
    public Player UnitOwner { get => unitOwner; set => unitOwner = value; }
    public Wargear Wargear { get => wargear; set => wargear = value; }

    public int UnitMove { get => unitMove; }
    public int UnitSpeed { get => unitSpeed; }

    public int UnitActions { get => unitActions; }
    public int UnitWounds { get => unitWounds; }
    public int UnitCourage { get => unitCourage; }

    public int UnitWill { get => unitWill; }
    public int UnitMight { get => unitMight; }
    #endregion

    private void Start()
    {
        this.name = unitBaseStats.name;
        unitWounds = unitBaseStats.unitWounds;

        ResetStats();
    }

    public void ResetStats()
    {
        unitMove = unitBaseStats.unitMove;
        unitSpeed = unitBaseStats.unitSpeed;
        unitActions = unitBaseStats.unitActions;

        unitFightSkill = unitBaseStats.unitFightSkill;
        unitArcherySkill = unitBaseStats.unitArcherySkill;
        unitStrength = unitBaseStats.unitStrength;
        unitDefence = unitBaseStats.unitDefence;

        unitActions = unitBaseStats.unitActions;
        unitCourage = unitBaseStats.unitCourage;

        unitWill = unitBaseStats.unitWill;
        unitMight = unitBaseStats.unitMight;
    }


    /// <summary>
    /// Returns defence value for unit
    /// </summary>
    /// <returns></returns>
    public int GetDefence()
    {
        var defence = unitDefence + wargear.armour.defence;
        return defence;
    }

    /// <summary>
    /// Returns strength value for unit
    /// </summary>
    /// <returns></returns>
    public int GetStrenght()
    {
        var strenght = unitStrength + wargear.combatWeapon.strength;
        return strenght;
    }

    /// <summary>
    /// Returns unit's melee fight value
    /// </summary>
    /// <returns></returns>
    public int GetMeleeFight()
    {
        var meleeFight = unitFightSkill;
        return meleeFight;
    }

    /// <summary>
    /// Returns unit's archery skill value
    /// </summary>
    /// <returns></returns>
    public int GetArcherySkill()
    {
        var archerySkill = unitArcherySkill;
        return archerySkill;
    }


    /// <summary>
    /// Subtract action points after performing an action.
    /// </summary>
    /// <param name="actionPoints"></param>
    public void ExecuteAction(int actionPoints) => unitActions -= actionPoints;

    /// <summary>
    /// Updates the health value and checks if the unit has survived
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        unitWounds -= damage;

        if (unitWounds <= 0)
            KillUnit();
        else
            OnGetDamage?.Invoke(this);
    }

    private void KillUnit()
    {
        OnDeath?.Invoke(this);

        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}