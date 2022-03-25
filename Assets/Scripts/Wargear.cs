using UnityEngine;

[CreateAssetMenu(fileName = "New Wargear", menuName = "Scriptable Objects/New Wargear")]
public class Wargear : ScriptableObject
{
    public enum RangeWeapons
    {
        None, Blowpipe, Bow, Crossbow, DwarfBow, DwarfLongbow, ElfBow, EsgarothBow, GreatBow,
        Longbow, OrcBow, ShortBow, Slingshot, ThrowingSpear, ThrowingWeapon, UrukhaiBow
    }

    public enum CloseCombatWeapon { None, SingleHandedWeapon, HandAndHalfHandedWeapon, TwoHandedWeapon }

    [System.Serializable]
    public struct MissileWeapon
    {
        public RangeWeapons type;
        public float range;
        public int strength;
    }

    [Header("Weapons")]
    public RangeWeapons missileWeapon;
    public CloseCombatWeapon closeCombatWeapon;
    [Space]
    public bool warSpear;
    public bool pike;
    public bool elvenMadeWeapon;
    public bool masterForgedWeapon;
    public bool stuffOfPower;
    public bool whip;

    [Header("Armor")]
    public bool armour;
    public bool heavyArmour;
    public bool heavyDwarfArmour;
    public bool shield;

    [Header("Equipment")]
    public bool banner;
    public bool elvenCloak;
    public bool warDrum;
    public bool warHorn;
    public bool theOneRing;
}