using System.Collections.Generic;
using UnityEngine;

public class PhaseShooting : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Script")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private Unit target;
    private int bowStrength = 2;        //temporary

    [SerializeField] private List<Obstacle> obstacles = new List<Obstacle>();

    [System.Serializable]
    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
        public int requiredToPass;
        public int rollResult;
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
            target = hit.transform.GetComponent<Unit>();
            if (hit.transform.CompareTag("Unit") && target.UnitOwner != activeUnit.UnitOwner)
            {
                if (activeUnit.shootAvailable && RollToWound.IsPossibleToAttack(target.unitDefence, bowStrength))
                {
                    activeUnit.shootAvailable = false;

                    RaycastObstacles();
                    ShootEffect();

                    activeUnit = null;
                    target = null;

                    obstacles.Clear();
                }
                else print("You can't attack this enemy!");
            }
            else target = null;
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
        {
            activeUnit = null;
            target = null;

            obstacles.Clear();
        }
    }

    private void RaycastObstacles()
    {
        activeUnit.transform.LookAt(target.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(activeUnit.transform.position + Vector3.up, activeUnit.transform.forward);

        foreach (var item in obstaclesHit)
        {
            var obst = new Obstacle();
            obst.obstacleName = item.collider.name;
            obst.obstacleDistance = item.distance;
            obst.requiredToPass = 3;                       // TEMPORARY
            obst.rollResult = RollTest.RollDiceD6();

            // Add obstacles to list
            if (obst.obstacleDistance <= Vector3.Distance(activeUnit.transform.position, target.transform.position))
                obstacles.Add(obst);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        var rollsPassed = 0;

        foreach (var obst in obstacles)
        {
            RollResultsPanel.instance.ShowResult(obst.rollResult, obst.obstacleName);
            if (obst.rollResult >= obst.requiredToPass)
                rollsPassed++;
        }

        if (rollsPassed == obstacles.Count)
        {
            if (RollToWound.GetWoundTest(activeUnit.unitDefence, bowStrength))
            {
                print($"{target.name} has been wounded!");
                target.GetDamage();
            }
            else
                print($"{target.name} reflected!");
        }
        else
            print($"{activeUnit.name} missed!");
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