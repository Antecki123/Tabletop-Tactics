using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Unit unit;

    [Header("Hashed Animations")]
    private static readonly int Move = Animator.StringToHash("move");
    private static readonly int Shoot = Animator.StringToHash("shoot");
    private static readonly int Death = Animator.StringToHash("death");
    private static readonly int GetHit = Animator.StringToHash("getHit");
    private static readonly int Attack = Animator.StringToHash("attack");

    private void OnEnable()
    {
        Movement.OnMovementAnimation += MovementAnimation;
        RangeAttack.OnShootAnimation += ShootAnimation;
        SingleAttack.OnAttackAnimation += AttackAnimation;
        SingleAttack.OnBlockAnimation += BlockAnimation;
        SingleAttack.OnAvoidAnimation += AvoidAnimation;
        Guard.OnGuardAnimation += GuardAnimation;

        Unit.OnGetDamage += GetHitAnimation;
        Unit.OnDeath += DeathAnimation;
    }
    private void OnDisable()
    {
        Movement.OnMovementAnimation -= MovementAnimation;
        RangeAttack.OnShootAnimation -= ShootAnimation;
        SingleAttack.OnAttackAnimation -= AttackAnimation;
        SingleAttack.OnBlockAnimation -= BlockAnimation;
        SingleAttack.OnAvoidAnimation -= AvoidAnimation;
        Guard.OnGuardAnimation -= GuardAnimation;

        Unit.OnGetDamage -= GetHitAnimation;
        Unit.OnDeath -= DeathAnimation;
    }

    private void MovementAnimation(Unit unit, bool state)
    {
        if (unit == this.unit)
            animator.SetBool(Move, state);
    }

    private void DeathAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger(Death);
    }

    private void GetHitAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger(GetHit);
    }

    private void ShootAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger(Shoot);
    }

    private void AttackAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger(Attack);
    }

    private void GuardAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger("guard");
    }

    // TODO
    private void BlockAnimation(Unit unit)
    {
        if (unit == this.unit) { print("Block"); }
        //animator.SetTrigger("block");
    }
    // TODO
    private void AvoidAnimation(Unit unit)
    {
        if (unit == this.unit) { print("Avoid"); }
        //animator.SetTrigger("avoid");
    }
}