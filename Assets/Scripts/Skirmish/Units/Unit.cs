using System;
using UnityEngine;

[RequireComponent(typeof(UnitAnimation)), SelectionBase]
public class Unit : MonoBehaviour
{
    public static Action<Unit> OnGetDamage;
    public static Action<Unit> OnDeath;

    public enum Player { Player1, Player2, AI }

    #region PROPERTIES
    [field: SerializeField] public UnitStats UnitBaseStats { get; internal set; }
    [field: SerializeField] public Player UnitOwner { get; internal set; }
    [field: SerializeField] public Wargear Wargear { get; internal set; }

    public int UnitMove { get; private set; }
    public int UnitSpeed { get; private set; }
    public int UnitActions { get; private set; }

    public int UnitFightSkill { get; private set; }
    public int UnitArcherySkill { get; private set; }
    public int UnitStrength { get; private set; }
    public int UnitDefence { get; private set; }

    public int UnitWounds { get; private set; }
    public int UnitCourage { get; private set; }

    public int UnitWill { get; private set; }
    public int UnitMight { get; private set; }
    #endregion

    private void Start()
    {
        this.name = UnitBaseStats.name;
        UnitWounds = UnitBaseStats.unitWounds;

        ResetStats();
    }

    public void ResetStats()
    {
        UnitMove = UnitBaseStats.unitMove;
        UnitSpeed = UnitBaseStats.unitSpeed;
        UnitActions = UnitBaseStats.unitActions;
        
        UnitFightSkill = UnitBaseStats.unitFightSkill;
        UnitArcherySkill = UnitBaseStats.unitArcherySkill;
        UnitStrength = UnitBaseStats.unitStrength;
        UnitDefence = UnitBaseStats.unitDefence;

        UnitCourage = UnitBaseStats.unitCourage;
        UnitWill = UnitBaseStats.unitWill;
        UnitMight = UnitBaseStats.unitMight;
    }


    /// <summary>
    /// Returns defence value for unit
    /// </summary>
    /// <returns></returns>
    public int GetDefence()
    {
        var defence = UnitDefence + Wargear.armour.defence;
        return defence;
    }

    /// <summary>
    /// Returns strength value for unit
    /// </summary>
    /// <returns></returns>
    public int GetStrenght()
    {
        var strenght = UnitStrength + Wargear.combatWeapon.strength;
        return strenght;
    }

    /// <summary>
    /// Returns unit's melee fight value
    /// </summary>
    /// <returns></returns>
    public int GetMeleeFight()
    {
        var meleeFight = UnitFightSkill;
        return meleeFight;
    }

    /// <summary>
    /// Returns unit's archery skill value
    /// </summary>
    /// <returns></returns>
    public int GetArcherySkill()
    {
        var archerySkill = UnitArcherySkill;
        return archerySkill;
    }

    /// <summary>
    /// Subtract action points after performing an action.
    /// </summary>
    /// <param name="actionPoints"></param>
    public void ExecuteAction(int actionPoints) => UnitActions -= actionPoints;

    /// <summary>
    /// Updates the health value and checks if the unit has survived
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        UnitWounds -= damage;

        if (UnitWounds <= 0)
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