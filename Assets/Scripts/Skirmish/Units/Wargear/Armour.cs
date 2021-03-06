using UnityEngine;

[CreateAssetMenu(fileName = "New Wargear", menuName = "Scriptable Objects/Wargear/New Armour")]
public class Armour : ScriptableObject
{
    public enum ArmourType { None, LightArmour, HeavyArmour, DwarfArmour }

    public ArmourType type;
    public int defence;
    public float weight;
    [Space]
    public GameObject armourPrefab;
}