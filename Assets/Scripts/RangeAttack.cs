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
                if (activeUnit.RangeWeapon.type != RangeWeapon.WeaponType.None && activeUnit.shootAvailable &&
                    WoundTest.IsPossibleToAttack(target.GetDefence(), activeUnit.RangeWeapon.strength) &&
                    activeUnit.RangeWeapon.range >= Vector3.Distance(activeUnit.transform.position, target.transform.position))
                {
                    //activeUnit.shootAvailable = false;
                    RaycastObstacles();
                    ShootEffect();

                    ClearAction();
                }
                else Debug.Log("You can't attack enemy!");
            }
            ClearAction();
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
            var obstacle = new Obstacle
            {
                obstacleName = hit.collider.name,
                obstacleDistance = hit.distance
            };

            // Add every obstacle between active unit and target to list
            if (obstacle.obstacleDistance <= Vector3.Distance(activeUnit.transform.position, target.transform.position))
                obstacles.Add(obstacle);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - 15 * obstacles.Count - 1 * Mathf.Round(Vector3.Distance(activeUnit.transform.position, target.transform.position));
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{activeUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), activeUnit.RangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
            }
        }
    }
}