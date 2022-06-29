using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Unit unit;

    private void OnEnable()
    {
        Movement.OnMovementAnimation += MovementAnimation;
        RangeAttack.OnShootAnimation += ShootAnimation;
        MeleeAttack.OnAttackAnimation += AttackAnimation;
        MeleeAttack.OnBlockAnimation += BlockAnimation;
        MeleeAttack.OnAvoidAnimation += AvoidAnimation;
        Guard.OnGuardAnimation += GuardAnimation;

        Unit.OnGetDamage += GetHitAnimation;
        Unit.OnDeath += DeathAnimation;
    }
    private void OnDisable()
    {
        Movement.OnMovementAnimation -= MovementAnimation;
        RangeAttack.OnShootAnimation -= ShootAnimation;
        MeleeAttack.OnAttackAnimation -= AttackAnimation;
        MeleeAttack.OnBlockAnimation -= BlockAnimation;
        MeleeAttack.OnAvoidAnimation -= AvoidAnimation;
        Guard.OnGuardAnimation -= GuardAnimation;

        Unit.OnGetDamage -= GetHitAnimation;
        Unit.OnDeath -= DeathAnimation;
    }

    private void MovementAnimation(Unit unit, bool state)
    {
        if (unit == this.unit)
            animator.SetBool("move", state);
    }

    private void DeathAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger("death");
    }

    private void GetHitAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger("getHit");
    }

    private void ShootAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger("shoot");
    }

    private void AttackAnimation(Unit unit)
    {
        if (unit == this.unit)
            animator.SetTrigger("attack");
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