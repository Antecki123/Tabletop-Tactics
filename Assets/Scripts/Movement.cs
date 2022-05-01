using UnityEngine;

public class Movement
{
    public static System.Action<Unit, GridNode> OnUnitChangePosition;

    [Header("Component References")]
    private readonly UnitActions unitActions;
    private readonly Camera mainCamera;

    private readonly int movementGridLayer = 1024;

    public Movement(UnitActions phaseAction)
    {
        this.unitActions = phaseAction;
        mainCamera = Camera.main;
    }
    
    public void UpdateAction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Highlight grid
        GridManager.instance.GetComponent<IHighlightGrid>().HighlightGridMovement(unitActions.ActiveUnit, unitActions.ActiveUnit.UnitMove, Color.blue);

        // Set destination
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, movementGridLayer))
        {
            var destination = hit.collider.GetComponent<GridNode>();

            if (destination.movementValue != 0 && !destination.isOccupied)
            {
                unitActions.ActiveUnit.navMeshAgent.destination = destination.transform.position;
                OnUnitChangePosition?.Invoke(unitActions.ActiveUnit, destination);

                unitActions.ActiveUnit.ExecuteAction(1);
                unitActions.FinishAction();
            }
            else unitActions.ActiveUnit.navMeshAgent.ResetPath();
        }

        // Clear active action
        if (Input.GetMouseButtonDown(1))
            unitActions.ClearAction();
    }
}