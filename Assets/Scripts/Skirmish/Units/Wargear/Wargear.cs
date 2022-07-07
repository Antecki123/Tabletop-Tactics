using UnityEngine;

[System.Serializable]
public class Wargear
{
    [Header("Weapons")]
    public RangeWeapon rangeWeapon;
    public CombatWeapon combatWeapon;
    public CombatWeapon secondaryWeapon;

    [Header("Armour")]
    public Armour armour;
    public bool shield;

    [Header("Equipment")]
    public bool banner;
    public bool warDrum;
    public bool warHorn;
    public bool firstAid;

    public bool theOneRing;
}