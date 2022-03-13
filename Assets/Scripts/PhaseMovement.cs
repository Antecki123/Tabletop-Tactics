using System.Collections;
using System.Collections.Generic;
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
        RaycastHit hit;

        // Set active unit
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && !activeUnit)
        {
            activeUnit = hit.transform.GetComponent<Unit>();
        }

        // Set target
        else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && activeUnit)
        {
            activeUnit.navMeshAgent.destination = hit.point;
            activeUnit = null;
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
        {
            activeUnit = null;
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }

    private void OnDrawGizmos()
    {
        if (activeUnit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(activeUnit.transform.position, activeUnit.unitMove);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeUnit.transform.position, 1f);
        }
    }
}
