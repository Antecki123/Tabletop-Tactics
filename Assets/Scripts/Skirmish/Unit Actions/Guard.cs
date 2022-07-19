using System;
using System.Threading.Tasks;
using UnityEngine;

public class Guard : MonoBehaviour
{
    #region Actions
    // Guard action
    public static Action<Unit> OnGuardAnimation;
    #endregion

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;

    private void OnEnable()
    {
        ExecuteAction();
    }

    public async void ExecuteAction()
    {
        var animationTime = 2000; //ms

        if (unitActions.State == UnitActions.UnitState.Idle)
        {
            unitActions.State = UnitActions.UnitState.ExecutingAction;

            unitActions.ActiveUnit.GuardAction(2);
            OnGuardAnimation?.Invoke(unitActions.ActiveUnit);

            await Task.Delay(animationTime);

            unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);

            unitActions.FinishAction();
            this.enabled = false;
        }
    }
}