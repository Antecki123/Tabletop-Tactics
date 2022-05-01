using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard
{
    [Header("Component References")]
    private UnitActions unitActions;

    public Guard(UnitActions phaseAction) =>  this.unitActions = phaseAction;

    public void UpdateAction()
    {
        //phaseAction.ActiveUnit.GuardAction();

        unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
        unitActions.FinishAction();
    }
}