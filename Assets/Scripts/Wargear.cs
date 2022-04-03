using UnityEngine;

public class Wargear : MonoBehaviour
{
    public enum WeaponAttribute { None, SingleHandedWeapon, HandAndHalfHandedWeapon, TwoHandedWeapon }

    [Header("Weapons")]
    public RangeWeapon rangeWeapon;
    public CombatWeapon combatWeapon;
    [Space]
    public WeaponAttribute weaponAttribute;
    public bool elvenMadeWeapon;
    public bool masterForgedWeapon;

    [Header("Armor")]
    public Armour armour;
    public bool shield;

    [Header("Equipment")]
    public bool banner;
    public bool elvenCloak;
    public bool warDrum;
    public bool warHorn;
    public bool theOneRing;
}