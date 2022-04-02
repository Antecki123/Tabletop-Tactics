using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private enum ActiveAction { MeleeAttack, RangeAttack, Support, ThrowWeapon, Reload }

    [Header("Component References")]
    public NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private UnitStats unitStats;

    [Header("Unit Properties")]
    [SerializeField] private PhaseManager.Player unitOwner;
    [SerializeField] private Wargear wargear;
    private Vector3 startPosition;

    [HideInInspector] public int unitMove;
    [HideInInspector] public int unitMeleeFight;
    [HideInInspector] public int unitRangeFight;
    [HideInInspector] public int unitStrength;
    [HideInInspector] public int unitDefence;
    [HideInInspector] public int unitAttacks;
    [HideInInspector] public int unitWounds;
    [HideInInspector] public int unitCourage;
    [HideInInspector] public int unitWill;
    [HideInInspector] public int unitMight;
    [HideInInspector] public int unitFate;

    [Space]
    public float moveLeft;
    public bool shootAvailable;
    public bool duelAvailable;

    [Header("Animations")]
    private bool move = false;
    //private bool shoot = false;

    public PhaseManager.Player UnitOwner { get => unitOwner; private set { } }
    public Vector3 StartPosition { get => startPosition; private set { } }
    public Wargear Wargear { get => wargear; private set { } }

    private void Start()
    {
        name = unitStats.name;
        startPosition = transform.position;
        moveLeft = unitStats.unitMove;

        unitMove = unitStats.unitMove;
        unitMeleeFight = unitStats.unitMeleeFight;
        unitRangeFight = unitStats.unitArcherySkill;
        unitStrength = unitStats.unitStrength;
        unitDefence = unitStats.unitDefence;
        unitAttacks = unitStats.unitAttacks;
        unitWounds = unitStats.unitWounds;
        unitCourage = unitStats.unitCourage;

        unitWill = unitStats.unitWill;
        unitMight = unitStats.unitMight;
        unitFate = unitStats.unitFate;

        ResetStats();
    }

    private void LateUpdate()
    {
        if (navMeshAgent.hasPath)
            move = true;
        else move = false;

        animator.SetBool("move", move);
    }

    public void ResetStats()
    {
        moveLeft = unitStats.unitMove;
        startPosition = transform.position;

        duelAvailable = true;
        shootAvailable = (wargear.missileWeapon != Wargear.RangeWeapons.None);
    }

    public void GetDamage()
    {
        unitWounds--;
        if (unitWounds <= 0)
        {
            animator.SetTrigger("death");
            print("Unit DEAD!");
        }
    }
}