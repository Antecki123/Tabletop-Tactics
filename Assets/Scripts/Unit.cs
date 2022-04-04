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

    [HideInInspector] public int unitMove;
    [HideInInspector] public int unitMeleeFight;
    [HideInInspector] public int unitRangeFight;
    [HideInInspector] public int unitStrength;
    [HideInInspector] public int unitDefence;
    [HideInInspector] public int unitActions;
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
    public RangeWeapon RangeWeapon { get => wargear.rangeWeapon; private set { } }

    private void Start()
    {
        name = unitStats.name;
        moveLeft = unitStats.unitMove;

        unitMove = unitStats.unitMove;
        unitMeleeFight = unitStats.unitMeleeFight;
        unitRangeFight = unitStats.unitArcherySkill;
        unitStrength = unitStats.unitStrength;
        unitDefence = unitStats.unitDefence;
        unitActions = unitStats.unitActions;
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

        duelAvailable = true;
        shootAvailable = (wargear.rangeWeapon.type != RangeWeapon.WeaponType.None);
    }

    /*
    public void GetDamage()
    {
        unitWounds--;
        if (unitWounds <= 0)
        {
            animator.SetTrigger("death");
            print("Unit DEAD!");
        }
    }*/

    /// <summary>
    /// Returns defence value for unit
    /// </summary>
    /// <returns></returns>
    public int GetDefence()
    {
        var defence = unitDefence + wargear.armour.defence;
        return defence;
    }

    /// <summary>
    /// Returns strength value for unit
    /// </summary>
    /// <returns></returns>
    public int GetStrenght()
    {
        var strenght = unitStrength + wargear.combatWeapon.strength;
        return strenght;
    }
}