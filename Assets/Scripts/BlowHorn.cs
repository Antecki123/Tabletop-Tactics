using UnityEngine;
using System.Threading.Tasks;

public class BlowHorn
{
    [Header("Component References")]
    private UnitActions unitActions;
    private int actionRange = 3;

    public BlowHorn(UnitActions phaseAction) => this.unitActions = phaseAction;

    public void UpdateAction()
    {
        // Highlight grid
        GridManager.instance.GetComponent<IHighlightGrid>().HighlightGridRange(unitActions.ActiveUnit, actionRange, Color.yellow);

        //phaseAction.ActiveUnit.BlowHornAction();

        unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
        unitActions.FinishAction();
    }
}