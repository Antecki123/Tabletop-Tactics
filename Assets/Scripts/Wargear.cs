using UnityEngine;

[System.Serializable]
public class Wargear
{
    [Header("Weapons")]
    public RangeWeapon rangeWeapon;
    public CombatWeapon combatWeapon;
    [Space]
    public bool elvenMadeWeapon;
    public bool masterForgedWeapon;

    [Header("Armour")]
    public Armour armour;
    public bool shield;

    [Header("Equipment")]
    public bool banner;
    public bool elvenCloak;
    public bool warDrum;
    public bool warHorn;
    public bool theOneRing;
}