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
    public int unitMove;
    public int unitMeleeFight;
    public int unitArcherySkill;
    public int unitStrength;
    public int unitDefence;
    public int unitAttacks;
    public int unitWounds;
    public int unitCourage;
    [Space]
    public int unitWill;
    public int unitMight;
    public int unitFate;

    [Header("Unit Prefabs")]
    public GameObject unitModel;
    public Sprite unitImage;
}