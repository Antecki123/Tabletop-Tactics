using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard
{
    [Header("Component References")]
    private PhaseActions phaseAction;

    public Guard(PhaseActions phaseAction) =>  this.phaseAction = phaseAction;

    public void UpdateAction()
    {
        phaseAction.ActiveUnit.GuardAction();

        phaseAction.ClearActiveAction();
    }
}