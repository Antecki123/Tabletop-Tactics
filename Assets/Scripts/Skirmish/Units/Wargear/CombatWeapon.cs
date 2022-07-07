using UnityEngine;

[CreateAssetMenu(fileName = "New Wargear", menuName = "Scriptable Objects/Wargear/New Combat Weapon")]
public class CombatWeapon : ScriptableObject
{
    public enum WeaponType { None, Sword, WarSpear, ThrowSpear, Pike, StuffOfPower, Whip, Mace, Knife, Club }

    public WeaponType type;
    public int strength;
    public int range;
    public float weight;
    [Space]
    public GameObject weaponPrefab;
}