using System.Collections;
using UnityEngine;

public class BlowHorn
{
    [Header("Component References")]
    private UnitActions unitActions;
    //private int actionRange = 2;

    public BlowHorn(UnitActions phaseAction) => this.unitActions = phaseAction;

    public void UpdateAction()
    {
        // Highlight grid with time duration
        HighlightCells();

        //phaseAction.ActiveUnit.BlowHornAction();

        unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
        //unitActions.FinishAction();
    }

    private async void HighlightCells()
    {
        var highlightDuration = 3000;

        //unitActions.gridBehaviour.HighlightGridRange(unitActions.ActiveUnit, actionRange, Color.yellow);
        await System.Threading.Tasks.Task.Delay(highlightDuration);

        //unitActions.gridBehaviour.ClearHighlight();
        unitActions.FinishAction();
    }
}