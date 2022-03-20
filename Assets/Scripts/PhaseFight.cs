using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseFight : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Fight Script")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private Unit target;
    [SerializeField] private List<Unit> targets = new List<Unit>();
    [Space]
    [SerializeField] private List<Unit> possibleTargets = new List<Unit>();
    [SerializeField] private List<Unit> possibleSupport = new List<Unit>();

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

                Collider[] hitColliders = Physics.OverlapSphere(activeUnit.transform.position, 1f);

                foreach (var item in hitColliders)
                {
                    var targetComponent = item.GetComponent<Unit>();

                    if (targetComponent && targetComponent.UnitOwner != activeUnit.UnitOwner)
                        possibleTargets.Add(item.GetComponent<Unit>());

                    else if (targetComponent && targetComponent.UnitOwner == activeUnit.UnitOwner)
                        possibleSupport.Add(item.GetComponent<Unit>());
                }
            }
        }

        // Attack target
        else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && activeUnit)
        {
            if (possibleTargets.Contains(hit.transform.GetComponent<Unit>()))
            {
                target = hit.transform.GetComponent<Unit>();
                activeUnit.transform.LookAt(target.transform.position);
                
                print("ATTACKING " + target.name);
            }
            else if (possibleSupport.Contains(hit.transform.GetComponent<Unit>()))
            {
                target = hit.transform.GetComponent<Unit>();
                activeUnit.transform.LookAt(target.transform.position);

                print("SUPPORTING " + target.name);
            }
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
        {
            activeUnit = null;
            target = null;

            possibleTargets.Clear();
            possibleSupport.Clear();
        }
    }

    // DEBUG ========================
    private void OnDrawGizmos()
    {
        if (activeUnit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(activeUnit.transform.position, MousePosition());

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeUnit.transform.position, 1f);
        }
        if (target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(activeUnit.transform.position + Vector3.up, target.transform.position + Vector3.up);
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}