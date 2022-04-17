using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Movement
{
    public static System.Action<Unit, GridNode> OnUnitChangePosition;

    [Header("Component References")]
    private PhaseActions phaseAction;
    private Camera mainCamera;

    [Header("Movement Script")]
    private Unit activeUnit;

    private readonly int movementGridLayer = 1024;

    public Movement(PhaseActions phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        if (!phaseAction.activeUnit)
            return;
        else if (!activeUnit)
            this.activeUnit = phaseAction.activeUnit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set position to move
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit, 100f, movementGridLayer) && activeUnit)
        {
            activeUnit.navMeshAgent.speed = 0f;
            activeUnit.navMeshAgent.destination = hit.collider.transform.position;

            var nextPosition = hit.collider.GetComponent<GridNode>();
            MoveUnitToPosition(nextPosition);
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
            ClearAction();
    }

    private void ClearAction()
    {
        activeUnit = null;

        phaseAction.activeUnit = null;
        phaseAction.activeAction = PhaseActions.UnitAction.None;
    }

    private async void MoveUnitToPosition(GridNode nexPosition)
    {
        while (activeUnit.navMeshAgent.remainingDistance == 0)
            await Task.Delay(1);

        var distance = 0f;
        for (int i = 0; i < activeUnit.navMeshAgent.path.corners.Length - 1; i++)
            distance += Vector3.Distance(activeUnit.navMeshAgent.path.corners[i], activeUnit.navMeshAgent.path.corners[i + 1]);

        if (distance <= activeUnit.moveLeft)    //&& !hexObject.GetComponent<GridHex>().isOccupied)
        {
            OnUnitChangePosition?.Invoke(activeUnit, nexPosition);

            activeUnit.navMeshAgent.speed = 1.5f;
            activeUnit.moveLeft -= distance;

            ClearAction();
        }
        else activeUnit.navMeshAgent.ResetPath();
    }
}