using System.Collections.Generic;
using UnityEngine;

public class RangeAttack
{
    [Header("Component References")]
    private Camera mainCamera;

    [Header("Shooting Script")]
    private Unit activeUnit;
    private Unit target;
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

    public RangeAttack() => mainCamera = Camera.main;

    public void UpdateAction(Unit activeUnit)
    {
        this.activeUnit = activeUnit;
        Debug.Log("Range Attack");

        /*Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
                else Debug.Log("You can't attack this enemy!");
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
        */
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
            //RollResultsPanel.instance.ShowResult(obst.rollResult, obst.obstacleName);
            if (obst.rollResult >= obst.requiredToPass)
                rollsPassed++;
        }

        if (rollsPassed == obstacles.Count)
        {
            if (RollToWound.GetWoundTest(activeUnit.unitDefence, bowStrength))
            {
                //print($"{target.name} has been wounded!");
                target.GetDamage();
            }
            else Debug.Log($"{target.name} reflected!");
        }
        else Debug.Log($"{activeUnit.name} missed!");
    }
}