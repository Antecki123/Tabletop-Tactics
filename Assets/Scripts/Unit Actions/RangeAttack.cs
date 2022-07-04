using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    #region Actions
    // Shoot Animation
    public static Action<Unit> OnShootAnimation;
    #endregion

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputsManager inputs;
    [Space]
    [SerializeField] private Camera mainCamera;
    [Space]
    [SerializeField] private FloatVariable probability;
    [SerializeField] private GameEvent updateProbability;

    private Unit targetUnit;
    private List<Obstacle> obstacles = new();

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell lastNode;

    private bool hitTargetEvent = false;
    private bool isTargetWounded = false;

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

        originNode = gridManager.GridCellsList.Find(n => n.Unit == unitActions.ActiveUnit);

        Projectile.OnTargetHit += ReturnShootResult;
    }
    private void OnDisable()
    {
        targetUnit = null;
        originNode = null;
        targetNode = null;
        obstacles.Clear();

        hitTargetEvent = false;
        isTargetWounded = false;

        VisualEfects.Instance.ArcMarker?.TurnOffMarker();
        VisualEfects.Instance.PositionMarker?.TurnOffMarker();

        Projectile.OnTargetHit -= ReturnShootResult;
    }

    public void Update()
    {
        // Clear action
        if (inputs.RightMouseButton && unitActions.State == UnitActions.UnitState.Idle)
        {
            VisualEfects.Instance.ArcMarker?.TurnOffMarker();
            VisualEfects.Instance.PositionMarker?.TurnOffMarker();

            this.enabled = false;
            return;
        }

        // Find target (set pointer)
        if ((targetNode = GetTargetNode()) && unitActions.State == UnitActions.UnitState.Idle)
        {
            if (targetNode != lastNode)
            {
                lastNode = targetNode;

                VisualEfects.Instance.ArcMarker?.TurnOnMarker(originNode, targetNode);
                VisualEfects.Instance.PositionMarker?.TurnOnMarker(originNode, targetNode);
            }
        }
        else
        {
            lastNode = null;

            VisualEfects.Instance.ArcMarker?.TurnOffMarker();
            VisualEfects.Instance.PositionMarker?.TurnOffMarker();
            return;
        }

        // Attack target
        if (inputs.LeftMouseButton && GetTargetNode() == targetNode && unitActions.State == UnitActions.UnitState.Idle)
        {
            if ((targetUnit = targetNode.Unit) && targetUnit.UnitOwner != unitActions.ActiveUnit.UnitOwner)
            {
                if (unitActions.ActiveUnit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None && !originNode.AdjacentCells.Contains(targetNode) &&
                    unitActions.ActiveUnit.Wargear.rangeWeapon.range >= Vector3.Distance(unitActions.ActiveUnit.transform.position, targetUnit.transform.position))
                {
                    unitActions.State = UnitActions.UnitState.ExecutingAction;
                    OnShootAnimation?.Invoke(unitActions.ActiveUnit);

                    ExecuteAction();
                    unitActions.ActiveUnit.ExecuteAction(unitActions.ActiveUnit.UnitActions);
                }
            }
            else Debug.Log("You can't attack this unit!");
        }
    }

    private async void ExecuteAction()
    {
        // wait for animation delay in millis
        var animationDelay = 2000;
        await System.Threading.Tasks.Task.Delay(animationDelay);

        RaycastObstacles();
        ShootCalculation();

        hitTargetEvent = true;

        while (hitTargetEvent)
            await System.Threading.Tasks.Task.Yield();

        if (isTargetWounded)
            targetUnit.GetDamage(1);

        unitActions.FinishAction();
        this.enabled = false;
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

    private void ShootCalculation()
    {
        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - (15 * obstacles.Count - 1) - (1 * Mathf.Round(Vector3.Distance(unitActions.ActiveUnit.transform.position, targetUnit.transform.position)));
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = hitResult < hitChance;

        //Debug.Log($"{unitActions.ActiveUnit.name} hit chance: {hitChance}% Hit result: {hitTarget}");

        if (hitTarget)
        {
            var woundTarget = WoundTest.GetWoundTest(targetUnit.GetDefence(), unitActions.ActiveUnit.Wargear.rangeWeapon.strength);
            if (woundTarget)
            {
                // do something when target has been wounded
                isTargetWounded = true;
                //targetUnit.GetDamage(1);
            }
        }

        probability.value = hitChance;
        InstantiateteProjectile(hitTarget);
    }

    private void InstantiateteProjectile(bool getHit)
    {
        var offset = Vector3.up;
        var activeUnit = unitActions.ActiveUnit;

        var projectile = Instantiate(activeUnit.Wargear.rangeWeapon.arrowPrefab,
            activeUnit.transform.position + offset, activeUnit.transform.rotation);

        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.startPoint = unitActions.ActiveUnit.transform.position + offset;

        projectileComponent.targetPoint = (getHit) ? targetUnit.transform.position + offset :
            targetUnit.transform.position + activeUnit.transform.forward * UnityEngine.Random.Range(-3, 10);
    }

    private void ReturnShootResult() => hitTargetEvent = false;

    private GridCell GetTargetNode()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(inputs.MousePosition), out RaycastHit hit, 100f, 1024);

        if (hit.collider)
            return hit.collider.GetComponent<GridCell>();
        else
            return null;
    }
}