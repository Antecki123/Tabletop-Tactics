using System;
using UnityEngine;

public class Guard : MonoBehaviour
{
    #region Actions
    // Guard action
    public static Action<Unit> OnGuardAnimation;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;

    private void OnEnable()
    {
        unitActions = GetComponent<UnitActions>();

        ExecuteAction();
    }

    public async void ExecuteAction()
    {
        var animationTime = 2000; //ms

        if (unitActions.State == UnitActions.UnitState.Idle)
        {
            unitActions.State = UnitActions.UnitState.ExecutingAction;

            //phaseAction.ActiveUnit.GuardAction();
            OnGuardAnimation?.Invoke(unitActions.ActiveUnit);

            await System.Threading.Tasks.Task.Delay(animationTime);

            unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
            unitActions.FinishAction();
            this.enabled = false;
        }
    }
}