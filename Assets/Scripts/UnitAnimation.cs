using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [Header("Component References")]
    private Animator animator;
    private Unit unit;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        RangeAttack.OnShootingAttack += ShootAnimation;
        MeleeAttack.OnAttack += AttackAnimation;
        MeleeAttack.OnBlock +=BlockAnimation;
        MeleeAttack.OnAvoid += AvoidAnimation;
        Guard.OnGuard += GuardAnimation;

        Unit.OnGetDamage += GetHitAnimation;
        Unit.OnDeath += DeathAnimation;
    }
    private void OnDisable()
    {
        RangeAttack.OnShootingAttack -= ShootAnimation;
        MeleeAttack.OnAttack -= AttackAnimation;
        MeleeAttack.OnBlock -=BlockAnimation;
        MeleeAttack.OnAvoid -= AvoidAnimation;
        Guard.OnGuard -= GuardAnimation;

        Unit.OnGetDamage -= GetHitAnimation;
        Unit.OnDeath -= DeathAnimation;
    }

    private void MovementAnimation(Unit unit, GridCell cell)
    {
        if (unit == this.unit)
            animator.SetBool("move", true);
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