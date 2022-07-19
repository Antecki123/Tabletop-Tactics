using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Objects/Wargear/New Unit")]
public class UnitStats : ScriptableObject
{
    [Header("Unit Profile")]
    public string unitName;

    [Header("Unit Statistics")]
    [Tooltip("Available range of movement.")] public int unitMove;
    [Tooltip("Determines the priority of movement for a given turn")] public int unitSpeed;
    [Tooltip("Remaining amonut of actions.")] public int unitActions;
    [Space]
    [Tooltip("Close range fight skill. Value used in Hit Test.")] public int unitFightSkill;
    [Tooltip("Range attacks skill. Value used in Hit Test.")] public int unitArcherySkill;
    [Tooltip("Strength of the unit. Value used in Wound Test.")] public int unitStrength;
    [Tooltip("Defence of the unit. Value used in Wound Test.")] public int unitDefence;
    [Space]
    [Tooltip("Points of health.")] public int unitHealth;
    [Tooltip("Required for Test of Courage.")] public int unitCourage;
    [Tooltip("Points of power. Only for hero tier.")] public int unitWill;
    [Tooltip("Magic points. Only for hero tier.")] public int unitMight;
    [Space]
    [Tooltip("Value required for AI calculations.")] public int AIValue;

    [Header("Unit Prefabs")]
    public GameObject unitModel;
    public Sprite unitImage;
}