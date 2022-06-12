using System;
using UnityEngine;

public class UIActionButtons : ScriptableObject
{
    public Action OnClickMovementAction;
    public Action OnClickRangeAttackAction;
    public Action OnClickMeleeAttackAction;
    public Action OnClickGuardAction;
    public Action OnClickCastSpellAction;

    public Action OnClearAction;

    public void Movement() => OnClickMovementAction?.Invoke();
    public void RangeAttack() => OnClickRangeAttackAction?.Invoke();
    public void MeleeAttack() => OnClickMeleeAttackAction?.Invoke();
    public void Guard() => OnClickGuardAction?.Invoke();
    public void CastSpell() => OnClickCastSpellAction?.Invoke();

    public void ClearActions() => OnClearAction?.Invoke();
}