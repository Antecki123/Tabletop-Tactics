using System.Collections.Generic;
using UnityEngine;

public class RangeAttack
{
    [Header("Component References")]
    [SerializeField] private PhaseAction phaseAction;
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Script")]
    [SerializeField] private Unit activeUnit;
    [SerializeField] private Unit target;
    [SerializeField] private List<Obstacle> obstacles = new();

    [System.Serializable]
    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
    }

    public RangeAttack(PhaseAction phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        //Debug.Log("Range Attack");

        if (!phaseAction.activeUnit)
            return;
        else if (!activeUnit)
            this.activeUnit = phaseAction.activeUnit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && activeUnit)
        {
            target = hit.transform.GetComponent<Unit>();

            if (hit.transform.CompareTag("Unit") && target.UnitOwner != activeUnit.UnitOwner)
            {
                //Debug.Log("Weapon strength: " + activeUnit.Wargear.rangeWeapon.strength);
                //Debug.Log("Defence: " + target.unitDefence);
                if (activeUnit.shootAvailable && WoundTest.IsPossibleToAttack(target.unitDefence, activeUnit.Wargear.rangeWeapon.strength))
                {
                    //activeUnit.shootAvailable = false;
                    RaycastObstacles();
                    ShootEffect();

                    ClearAction();
                }
                else Debug.Log("You can't hurt this enemy!");
            }
            else target = null;
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && activeUnit)
            ClearAction();
    }

    private void ClearAction()
    {
        activeUnit = null;
        target = null;
        obstacles.Clear();

        phaseAction.activeUnit = null;
        phaseAction.activeAction = PhaseAction.UnitAction.None;
    }

    private void RaycastObstacles()
    {
        activeUnit.transform.LookAt(target.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(activeUnit.transform.position + Vector3.up, activeUnit.transform.forward);

        foreach (var hit in obstaclesHit)
        {
            var obstacle = new Obstacle();
            obstacle.obstacleName = hit.collider.name;
            obstacle.obstacleDistance = hit.distance;

            // Add every obstacle between active unit and target to list
            if (obstacle.obstacleDistance <= Vector3.Distance(activeUnit.transform.position, target.transform.position))
                obstacles.Add(obstacle);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        var woundTarget = false;

        var hitChance = 100 - 15 * obstacles.Count;         //var hitChance = 50 / obstacles.Count;
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult >= hitChance);

        Debug.Log($"Hit chance: {hitChance}% Hit result: {hitResult}%");

        if (hitTarget)
        {
            woundTarget = WoundTest.GetWoundTest(target.unitDefence, activeUnit.Wargear.rangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
            }
        }

        Debug.Log($"Target hit: {hitTarget}, target wounded: {woundTarget}");
    }
}