using System;
using UnityEngine;

public class Guard : MonoBehaviour
{
    #region Actions
    // Guard action
    public static Action<Unit> OnGuard;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;

    private void OnEnable()
    {
        unitActions = GetComponent<UnitActions>();

        ExecuteAction();
    }

    public void ExecuteAction()
    {
        if (unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            unitActions.ActiveUnit.Action = Unit.CurrentAction.Guard;

            //phaseAction.ActiveUnit.GuardAction();
            OnGuard?.Invoke(unitActions.ActiveUnit);

            unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
            unitActions.FinishAction();
            this.enabled = false;
        }
    }
}