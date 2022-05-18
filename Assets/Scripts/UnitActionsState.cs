using UnityEngine;

public abstract class UnitActionsState
{
    //public abstract void EnterState(UnitActions unitActions);
    public abstract void UpdateState(UnitActions unitActions);
}



public class MovementState : UnitActionsState
{
    private readonly int movementGridLayer = 1024;

    public override void UpdateState(UnitActions unitActions)
    {
        //unitActions.pathfinding.FindPath()
        Debug.Log("UpdateState MovementState");

        //WaitForClick();
    }

    private async void WaitForClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        while (!Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, movementGridLayer))
        {
            await System.Threading.Tasks.Task.Delay(10);
            Debug.Log("AWAITED");
        }
    }
}