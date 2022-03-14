using System.Collections;
using UnityEngine;

public class PhaseMovement : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Movement Script")]
    [SerializeField] private Unit activeUnit;

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && !activeUnit)
        {
            if (hit.transform.CompareTag("Unit"))
            {
                activeUnit = hit.transform.GetComponent<Unit>();
                if (activeUnit.UnitOwner != PhaseManager.instance.activePlayer)
                    activeUnit = null;
            }
        }

        // Set position to move
        else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && activeUnit)
        {
            activeUnit.navMeshAgent.speed = 0f;
            activeUnit.navMeshAgent.destination = hit.point;

            StartCoroutine(MoveUnitToPosition());
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
            activeUnit = null;

        // ==============================================
        if (activeUnit && activeUnit.navMeshAgent.hasPath)
        {
            lineRenderer.positionCount = activeUnit.navMeshAgent.path.corners.Length;
            for (int i = 0; i < activeUnit.navMeshAgent.path.corners.Length; i++)
                lineRenderer.SetPosition(i, activeUnit.navMeshAgent.path.corners[i]);
            lineRenderer.enabled = true;
        }
        else
            lineRenderer.enabled = false;
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }

    private IEnumerator MoveUnitToPosition()
    {
        while (activeUnit.navMeshAgent.remainingDistance == 0)
            yield return new WaitForEndOfFrame();

        var distance = 0f;
        for (int i = 0; i < activeUnit.navMeshAgent.path.corners.Length - 1; i++)
            distance += Vector3.Distance(activeUnit.navMeshAgent.path.corners[i], activeUnit.navMeshAgent.path.corners[i + 1]);

        if (distance <= activeUnit.MoveLeft)
        {
            activeUnit.navMeshAgent.speed = 1.5f;
            activeUnit.MoveLeft -= distance;
            activeUnit = null;
        }
        else activeUnit.navMeshAgent.ResetPath();
    }

    private void OnDrawGizmos()
    {
        if (activeUnit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(activeUnit.transform.position, activeUnit.MoveLeft);
            Gizmos.DrawLine(activeUnit.transform.position, MousePosition());

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeUnit.transform.position, 1f);
        }
    }
}