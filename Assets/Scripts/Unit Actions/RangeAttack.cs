using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    #region Actions
    // Attack
    public static Action<Unit> OnShootingAttack;
    // Turn on pointer
    public static Action<GridCell, GridCell> OnFindingTarget;
    // Turn off pointer
    public static Action OnClearAction;
    #endregion

    [Header("Component References")]
    private UnitActions unitActions;
    private Camera mainCamera;

    private Unit targetUnit;
    private List<Obstacle> obstacles = new();

    private GridCell originNode;
    private GridCell targetNode;

    [Serializable]
    private struct Obstacle
    {
        public string obstacleName;
        public float obstacleDistance;
        public Vector3 obstacleHitPoint;
    }

    private void OnEnable()
    {
        unitActions = GetComponent<UnitActions>();
        mainCamera = Camera.main;

        originNode = GridManager.instance.GridNodes.Find(n => n.Unit == unitActions.ActiveUnit);
    }
    private void OnDisable()
    {
        targetUnit = null;
        originNode = null;
        targetNode = null;
        obstacles.Clear();

        OnClearAction?.Invoke();
    }

    public void Update()
    {
        // Clear action
        if (Input.GetMouseButtonDown(1) && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            this.enabled = false;
            return;
        }

        // Find target (set pointer)
        if ((targetNode = GetTargetNode()) && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            OnFindingTarget?.Invoke(originNode, targetNode);
        }
        else
        {
            OnClearAction?.Invoke();
            return;
        }

        // Attack target
        if (Input.GetMouseButtonDown(0) && GetTargetNode() == targetNode && unitActions.ActiveUnit.Action == Unit.CurrentAction.None)
        {
            if ((targetUnit = targetNode.Unit) && targetUnit.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                if (unitActions.ActiveUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None && !originNode.AdjacentCells.Contains(targetNode) &&
                    unitActions.ActiveUnit.Wargear.rangeWeapon.range >= Vector3.Distance(unitActions.ActiveUnit.transform.position, targetUnit.transform.position))
                {
                    unitActions.ActiveUnit.Action = Unit.CurrentAction.RangeAttack;
                    OnShootingAttack?.Invoke(unitActions.ActiveUnit);

                    RaycastObstacles();
                    ShootEffect();

                    unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                    unitActions.FinishAction();

                    this.enabled = false;
                    return;
                }
            }
            else Debug.Log("You can't attack this enemy!");
        }
    }

    private void RaycastObstacles()
    {
        unitActions.ActiveUnit.transform.LookAt(targetUnit.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(unitActions.ActiveUnit.transform.position + Vector3.up, unitActions.ActiveUnit.transform.forward);

        foreach (var hit in obstaclesHit)
        {
            var obstacle = new Obstacle
            {
                obstacleName = hit.collider.name,
                obstacleDistance = hit.distance,
                obstacleHitPoint = hit.point
            };

            // Add every obstacle between active unit and target to list
            if (obstacle.obstacleDistance <= Vector3.Distance(unitActions.ActiveUnit.transform.position, targetUnit.transform.position))
                obstacles.Add(obstacle);
        }

        // Sort obstacles by distance from shooter
        obstacles.Sort((a, b) => a.obstacleDistance.CompareTo(b.obstacleDistance));
    }

    private void ShootEffect()
    {
        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - (15 * obstacles.Count - 1) - (1 * Mathf.Round(Vector3.Distance(unitActions.ActiveUnit.transform.position, targetUnit.transform.position)));
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = (hitResult < hitChance);

        Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(targetUnit.GetDefence(), unitActions.ActiveUnit.Wargear.rangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
                targetUnit.GetDamage(1);
            }
        }
    }

    private GridCell GetTargetNode()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, 1024);

        if (hit.collider)
            return hit.collider.GetComponent<GridCell>();
        else
            return null;
    }
}