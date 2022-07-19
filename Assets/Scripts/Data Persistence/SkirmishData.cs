using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skirmish Data", menuName = "Scriptable Objects/Utilities/Skirmish Data")]
public class SkirmishData : ScriptableObject
{
    [System.Serializable]
    public struct UnitsToSpawn
    {
        public UnitStats unitStats;
        public Unit.Player player;
        [Space]
        public Unit.Army unitArmy;
        public Unit.Type unitType;
        public Unit.HeroicTier unitHeroicTier;
        public Unit.Class unitClass;
        [Space]
        public Wargear wargear;
    }

    public enum MapSize { XS, S, M, L, XL, XXL }

    [Header("Battle Generator Settings")]
    public MapSize mapSize;
    [Space]
    [Range(0, 100)] public int obstaclesDensity;
    public List<GameObject> obstaclesList;
    [Space]
    public List<UnitsToSpawn> unitsToSpawn;
}
