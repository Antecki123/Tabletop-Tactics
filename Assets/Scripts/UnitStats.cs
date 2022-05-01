using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Objects/New Unit")]
public class UnitStats : ScriptableObject
{
    public enum UnitRace
    {
        Hobbit, Wazard, Man, Elf, Dwarf, Pony, SiegeEngine, Spirit, Ent, Eagle,Spider, 
        Orc, Troll, Warg, UrukHai, GreatBeast, Goblin, Dragon, Drake, Kraken, Bat, Mumak
    }

    public enum UnitArmy
    {
        ArmyOfThror, TheDeadOfDunharrow, TheFellowship, GarrisonOfDale, HallsOfThranduil,
        IronHills, Lothlorien, MinasTirith, Numenor, Rivendell, Rohan, SurvivorsOfLakeTown,
        ThorinsCompany, Angmar, AzogsLegion, DeselatorOfTheNorth, GoblinTown, Isengard, Mordor,
        Moria, TheSerpentHorde, TheTrolls
    }

    public enum UnitType { Infantry, Cavalry, Monster }
    public enum UnitHeroicTier { Warrior, HeroOfLegend, HeroOfValour, HeroOfFortitude, MinorHero, IndependentHero }

    [Header("Unit Profile")]
    public string unitName;
    public UnitRace unitRace;
    public UnitArmy unitArmy;
    public UnitType unitType;
    public UnitHeroicTier unitHeroicTier;

    [Header("Unit Statistics")]
    [Tooltip("Available range of movement.")]public int unitMove;
    [Tooltip("Determines the priority of movement for a given turn")] public int unitSpeed;
    [Tooltip("Close range fight skill. Value used in Hit Test.")] public int unitFightSkill;
    [Tooltip("Range attacks skill. Value used in Hit Test.")] public int unitArcherySkill;
    [Tooltip("Strength of the unit. Value used in Wound Test.")] public int unitStrength;
    [Tooltip("Defence of the unit. Value used in Wound Test.")] public int unitDefence;
    [Tooltip("Remaining amonut of actions.")] public int unitActions;
    [Tooltip("Points of health.")] public int unitWounds;
    [Tooltip("Required for Test of Courage.")] public int unitCourage;
    [Space]
    [Tooltip("Points of power. Only for hero tier.")] public int unitWill;
    [Tooltip("Magic points. Only for hero tier.")] public int unitMight;
    [Tooltip("Only for hero tier.")] public int unitFate;       // probably to delete

    [Header("Unit Prefabs")]
    public GameObject unitModel;
    public Sprite unitImage;
}