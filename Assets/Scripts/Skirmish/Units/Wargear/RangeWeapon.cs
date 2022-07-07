using UnityEngine;

[CreateAssetMenu(fileName = "New Wargear", menuName = "Scriptable Objects/Wargear/New Range Weapon")]
public class RangeWeapon : ScriptableObject
{
    public enum WeaponType
    {
        None, Blowpipe, Bow, Crossbow, DwarfBow, DwarfLongbow, ElfBow, EsgarothBow, GreatBow,
        Longbow, OrcBow, ShortBow, Slingshot, ThrowingSpear, ThrowingWeapon, UrukhaiBow
    }

    public WeaponType type;
    public int strength;
    public float range;
    public float weight;
    [Space]
    public GameObject weaponPrefab;
    public GameObject arrowPrefab;
}