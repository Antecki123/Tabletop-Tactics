using System.Collections.Generic;
using UnityEngine;

public class RangeAttack
{
    public static System.Action<Unit> OnShootingAttack;

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private Camera mainCamera;

    [Header("Shooting Script")]
    [SerializeField] private Unit target;
    [SerializeField] private List<Obstacle> obstacles = new();

    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
    }

    public RangeAttack(UnitActions phaseAction)
    {
        this.unitActions = phaseAction;
        mainCamera = Camera.main;
    }

    public void UpdateAction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Highlight grid
        GridManager.instance.GetComponent<IHighlightGrid>().HighlightGridRange(unitActions.ActiveUnit, (int)unitActions.ActiveUnit.Wargear.rangeWeapon.range, Color.cyan);

        // Set target
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit) && unitActions.ActiveUnit)
        {
            target = hit.transform.GetComponent<Unit>();

            if (hit.transform.CompareTag("Unit") && target.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                if (unitActions.ActiveUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None &&
                    unitActions.ActiveUnit.Wargear.rangeWeapon.range >= Vector3.Distance(unitActions.ActiveUnit.transform.position, target.transform.position))
                {
                    OnShootingAttack?.Invoke(unitActions.ActiveUnit);

                    RaycastObstacles();
                    ShootEffect();

                    obstacles.Clear();

                    unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                    unitActions.FinishAction();
                }
                else Debug.Log("You can't attack enemy!");
            }
        }

        // Clear active action
        if (Input.GetMouseButtonDown(1))
            unitActions.ClearAction();
    }

    private void RaycastObstacles()
    {
        unitActions.ActiveUnit.transform.LookAt(target.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(unitActions.ActiveUnit.transform.position + Vector3.up, unitActions.ActiveUnit.transform.forward);

        foreach (var hit in obstaclesHit)
        {
            var obstacle = new Obstacle
            {
                obstacleName = hit.collider.name,
                obstacleDistance = hit.distance
            };

            // Add every obstacle between active unit and target to list
            if (obstacle.obstacleDistance <= Vector3.Distance(unitActions.ActiveUnit.transform.position, target.transform.position))
                obstacles.Add(obstacle);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - (15 * obstacles.Count - 1) - (1 * Mathf.Round(Vector3.Distance(unitActions.ActiveUnit.transform.position, target.transform.position)));
        var hitResult = Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(target.GetDefence(), unitActions.ActiveUnit.Wargear.rangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
                target.GetDamage(1);
            }
        }
    }
}