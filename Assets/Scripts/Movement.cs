using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Movement
{
    public static System.Action<Unit, GridNode> OnUnitChangePosition;

    [Header("Component References")]
    private PhaseActions phaseAction;
    private readonly Camera mainCamera;

    private readonly int movementGridLayer = 1024;

    public Movement(PhaseActions phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        if (!phaseAction.ActiveUnit)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set position to move
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, movementGridLayer) && phaseAction.ActiveUnit)
        {
            phaseAction.ActiveUnit.navMeshAgent.speed = 0f;
            phaseAction.ActiveUnit.navMeshAgent.destination = hit.collider.transform.position;

            var nextPosition = hit.collider.GetComponent<GridNode>();
            MoveUnitToPosition(nextPosition);
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && phaseAction.ActiveUnit)
            phaseAction.ClearActiveAction();
    }

    private async void MoveUnitToPosition(GridNode nexPosition)
    {
        while (phaseAction.ActiveUnit.navMeshAgent.remainingDistance == 0)
            await Task.Delay(1);

        var distance = 0f;
        for (int i = 0; i < phaseAction.ActiveUnit.navMeshAgent.path.corners.Length - 1; i++)
            distance += Vector3.Distance(phaseAction.ActiveUnit.navMeshAgent.path.corners[i], phaseAction.ActiveUnit.navMeshAgent.path.corners[i + 1]);

        if (distance <= phaseAction.ActiveUnit.MoveLeft && !nexPosition.isOccupied)
        {
            OnUnitChangePosition?.Invoke(phaseAction.ActiveUnit, nexPosition);

            phaseAction.ActiveUnit.navMeshAgent.speed = 1.5f;
            phaseAction.ActiveUnit.MoveLeft -= distance;

            phaseAction.ClearActiveAction();
        }
        else phaseAction.ActiveUnit.navMeshAgent.ResetPath();
    }
}