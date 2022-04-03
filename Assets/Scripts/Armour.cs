using UnityEngine;

[CreateAssetMenu(fileName = "New Wargear", menuName = "Scriptable Objects/New Armour")]
public class Armour : ScriptableObject
{
    public enum ArmourType { None, LightArmour, HeavyArmour, DwarfArmour }

    public ArmourType type;
    public int defence;
}
