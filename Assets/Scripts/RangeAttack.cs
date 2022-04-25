using System.Collections.Generic;
using UnityEngine;

public class RangeAttack
{
    [Header("Component References")]
    [SerializeField] private PhaseActions phaseAction;
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Script")]
    [SerializeField] private Unit target;
    [SerializeField] private List<Obstacle> obstacles = new();

    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
    }

    public RangeAttack(PhaseActions phaseAction)
    {
        this.phaseAction = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        if (!phaseAction.ActiveUnit)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && phaseAction.ActiveUnit)
        {
            target = hit.transform.GetComponent<Unit>();

            if (hit.transform.CompareTag("Unit") && target.UnitOwner != phaseAction.ActiveUnit.UnitOwner)
            {
                if (phaseAction.ActiveUnit.RangeWeapon.type != RangeWeapon.WeaponType.None && phaseAction.ActiveUnit.ShootAvailable &&
                    WoundTest.IsPossibleToAttack(target.GetDefence(), phaseAction.ActiveUnit.RangeWeapon.strength) &&
                    phaseAction.ActiveUnit.RangeWeapon.range >= Vector3.Distance(phaseAction.ActiveUnit.transform.position, target.transform.position))
                {
                    //activeUnit.shootAvailable = false;
                    RaycastObstacles();
                    ShootEffect();

                    ClearAction();
                }
                else Debug.Log("You can't attack enemy!");
            }
            //ClearAction();
        }

        // Clear active unit
        if (Input.GetMouseButtonDown(1) && phaseAction.ActiveUnit)
            ClearAction();
    }

    private void ClearAction()
    {
        target = null;
        obstacles.Clear();
        phaseAction.ClearActiveAction();
    }

    private void RaycastObstacles()
    {
        phaseAction.ActiveUnit.transform.LookAt(target.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(phaseAction.ActiveUnit.transform.position + Vector3.up, phaseAction.ActiveUnit.transform.forward);

        foreach (var hit in obstaclesHit)
        {
            var obstacle = new Obstacle
            {
                obstacleName = hit.collider.name,
                obstacleDistance = hit.distance
            };

            // Add every obstacle between active unit and target to list
            if (obstacle.obstacleDistance <= Vector3.Distance(phaseAction.ActiveUnit.transform.position, target.transform.position))
                obstacles.Add(obstacle);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - 15 * obstacles.Count - 1 * Mathf.Round(Vector3.Distance(phaseAction.ActiveUnit.transform.position, target.transform.position));
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{phaseAction.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), phaseAction.ActiveUnit.RangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
            }
        }
    }
}