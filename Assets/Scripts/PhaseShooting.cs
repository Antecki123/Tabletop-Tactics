using System.Collections.Generic;
using UnityEngine;

public class PhaseShooting : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Script")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private Unit target;

    [SerializeField] private List<Obstacle> obstacles = new List<Obstacle>();

    [System.Serializable]
    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
        public float requiredToPass;
        public float rollResult;
    }

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

        // Set target
        else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) && activeUnit)
        {
            var hitTarget = hit.transform.GetComponent<Unit>();
            if (hit.transform.CompareTag("Unit") && hitTarget.UnitOwner != activeUnit.UnitOwner)
            {
                target = hitTarget;
                activeUnit.transform.LookAt(target.transform.position);

                RaycastHit[] obstaclesHit = Physics.RaycastAll(activeUnit.transform.position + Vector3.up, activeUnit.transform.forward);

                foreach (var item in obstaclesHit)
                {
                    var obst = new Obstacle();
                    obst.obstacleName = item.collider.name;
                    obst.obstacleDistance = item.distance;
                    obst.requiredToPass = 3f;                       // TEMPORARY
                    obst.rollResult = RollTest.RollDiceD6();

                    // Add obstacles to list
                    if (obst.obstacleDistance <= Vector3.Distance(activeUnit.transform.position, target.transform.position))
                        obstacles.Add(obst);
                }

                // Sort obstacles by distance from shooter
                obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
            }
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
        {
            activeUnit = null;
            target = null;

            obstacles.Clear();
        }
    }

    // DEBUG ========================
    private void OnDrawGizmos()
    {
        if (activeUnit)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(activeUnit.transform.position, 24f /* waepon range*/);
            Gizmos.DrawLine(activeUnit.transform.position + Vector3.up, MousePosition());

            if (target)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(activeUnit.transform.position + Vector3.up, target.transform.position + Vector3.up);
            }
        }
    }
    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}