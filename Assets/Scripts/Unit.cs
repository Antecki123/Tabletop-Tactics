using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(UnitAnimation))]
public class Unit : MonoBehaviour
{
    [Header("Component References")]
    public NavMeshAgent navMeshAgent;
    [SerializeField] private UnitStats unitStats;

    [Header("Unit Properties")]
    [SerializeField] private PhaseManager.Player unitOwner;
    [SerializeField] private Vector3 startPosition;
    [Space]
    [HideInInspector] public int unitMove;
    [HideInInspector] public int unitMeleeFight;
    [HideInInspector] public int unitRangeFight;
    [HideInInspector] public int unitStrength;
    [HideInInspector] public int unitDefence;
    [HideInInspector] public int unitAttacks;
    [HideInInspector] public int unitWounds;
    [HideInInspector] public int unitCourage;
    [Space]
    public int unitWill;
    public int unitMight;
    public int unitFate;

    [Space]
    private bool shootAvailable;
    private bool duelAvailable;

    public PhaseManager.Player UnitOwner { get => unitOwner; private set { } }
    public Vector3 StartPosition { get => startPosition; private set { } }


    private void Start()
    {
        name = unitStats.name;

        unitMove = unitStats.unitMove;
        unitMeleeFight = unitStats.unitMeleeFight;
        unitRangeFight = unitStats.unitRangeFight;
        unitStrength = unitStats.unitStrength;
        unitDefence = unitStats.unitDefence;
        unitAttacks = unitStats.unitAttacks;
        unitWounds = unitStats.unitWounds;
        unitCourage = unitStats.unitCourage;

        unitWill = unitStats.unitWill;
        unitMight = unitStats.unitMight;
        unitFate = unitStats.unitFate;
    }

    public void ResetStats()
    {
        unitMove = unitStats.unitMove;
        startPosition = transform.position;

        shootAvailable = true;
        duelAvailable = true;
    }
}