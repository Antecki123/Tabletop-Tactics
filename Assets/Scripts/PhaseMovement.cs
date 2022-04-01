using System.Collections;
using UnityEngine;

public class PhaseMovement : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Movement Script")]
    [SerializeField] private Unit activeUnit;

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && !activeUnit)
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
            //activeUnit.navMeshAgent.destination = hit.point;

            var roundCoordinate = new Vector2(Mathf.Round(hit.point.x), Mathf.Round(hit.point.z));
            activeUnit.navMeshAgent.destination = new Vector3(roundCoordinate.x, hit.point.y, roundCoordinate.y);

            StartCoroutine(MoveUnitToPosition());
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
            activeUnit = null;
    }

    private IEnumerator MoveUnitToPosition()
    {
        while (activeUnit.navMeshAgent.remainingDistance == 0)
            yield return new WaitForEndOfFrame();

        var distance = 0f;
        for (int i = 0; i < activeUnit.navMeshAgent.path.corners.Length - 1; i++)
            distance += Vector3.Distance(activeUnit.navMeshAgent.path.corners[i], activeUnit.navMeshAgent.path.corners[i + 1]);

        if (distance <= activeUnit.moveLeft)
        {
            activeUnit.navMeshAgent.speed = 1.5f;
            activeUnit.moveLeft -= distance;
            activeUnit = null;
        }
        else activeUnit.navMeshAgent.ResetPath();
    }

    // DEBUG ========================
    private void OnDrawGizmos()
    {
        if (activeUnit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(activeUnit.transform.position, activeUnit.moveLeft);
            Gizmos.DrawLine(activeUnit.transform.position, MousePosition());

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeUnit.transform.position, 1f);
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}