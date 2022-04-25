using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private enum ActiveAction { MeleeAttack, RangeAttack, Support, ThrowWeapon, Reload }

    [Header("Component References")]
    public NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private UnitStats unitStats;

    [Header("Unit Stats")]
    [SerializeField] private PhaseManager.Player unitOwner;
    [SerializeField] private Wargear wargear;

    private int unitMove;
    private int unitMeleeFight;
    private int unitRangeFight;
    private int unitStrength;
    private int unitDefence;
    private int unitActions;
    private int unitWounds;
    private int unitCourage;
    public int unitSpeed;              // TODO: add speed stat to scripptables obj

    private int unitWill;
    private int unitMight;
    private int unitFate;

    [Header("Unit Properties")]
    private float moveLeft;
    private int remainingActions;
    private bool shootAvailable;
    private bool duelAvailable;

    [Header("Animations")]
    private bool move = false;
    //private bool shoot = false;

    public int RemainingActions { get => remainingActions; set => remainingActions = value; }
    public float MoveLeft { get => moveLeft; set => moveLeft = value; }
    public bool ShootAvailable { get => shootAvailable; set => shootAvailable = value; }
    public bool DuelAvailable { get => duelAvailable; set => duelAvailable = value; }

    public PhaseManager.Player UnitOwner { get => unitOwner; private set { } }
    public RangeWeapon RangeWeapon { get => wargear.rangeWeapon; private set { } }

    private void Start()
    {
        name = unitStats.name;

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

        remainingActions = 2;
        moveLeft = unitStats.unitMove;

        duelAvailable = true;
        shootAvailable = (wargear.rangeWeapon.type != RangeWeapon.WeaponType.None);
    }

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

    /// <summary>
    /// Returns unit's melee fight value
    /// </summary>
    /// <returns></returns>
    public int GetMeleeFight()
    {
        var meleeFight = unitMeleeFight;
        return meleeFight;
    }

    public void GuardAction()
    {
        unitDefence++;
    }
}