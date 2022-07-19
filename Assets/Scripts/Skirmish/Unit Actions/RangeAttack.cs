using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [Header("UI References")]
    [SerializeField] private FloatVariable hitProbability;
    [SerializeField] private GameEvent updateProbability;

    private GridCell originNode;
    private GridCell targetNode;
    private GridCell lastNode;

    private bool hitTargetEvent = false;

    private void OnEnable()
    {
        originNode = gridManager.GridCellsList.Find(n => n.Unit == unitActions.ActiveUnit);

        VisualEfects.Instance.GridHighlight.TurnOnHighlightSimpleRange(originNode, (int)originNode.Unit.Wargear.rangeWeapon.range);

        Projectile.OnTargetHit += ReturnShootResult;
    }
    private void OnDisable()
    {
        originNode = null;
        targetNode = null;

        hitTargetEvent = false;

        VisualEfects.Instance.ArcMarker?.TurnOffMarker();
        VisualEfects.Instance.PositionMarker?.TurnOffMarker();
        VisualEfects.Instance.GridHighlight.TurnOffHighlight();

        Projectile.OnTargetHit -= ReturnShootResult;
    }

    private void Update()
    {
        // Clear action
        if (inputs.RightMouseButton && unitActions.State == UnitActions.UnitState.Idle)
        {
            this.enabled = false;
            return;
        }

        // Calculations
        if (unitActions.State == UnitActions.UnitState.Idle && (targetNode = GetTargetNode()) && targetNode.Unit &&
            targetNode.Unit.UnitOwner != originNode.Unit.UnitOwner && targetNode.BlockValue > 0)
        {
            if (targetNode != lastNode)
            {
                lastNode = targetNode;

                VisualEfects.Instance.ArcMarker?.TurnOnMarker(originNode, targetNode);
                VisualEfects.Instance.PositionMarker?.TurnOnMarker(originNode, targetNode);

                ShootCalculations();
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
        if (inputs.LeftMouseButton && unitActions.State == UnitActions.UnitState.Idle &&
            GetTargetNode() == targetNode && targetNode.BlockValue > 0)
        {
            if (originNode.Unit.Wargear.rangeWeapon.type != RangeWeapon.WeaponType.None)
            {
                unitActions.State = UnitActions.UnitState.ExecutingAction;
                OnShootAnimation?.Invoke(originNode.Unit);

                ExecuteAction();
                originNode.Unit.ExecuteAction(originNode.Unit.UnitActions);
            }
        }
    }

    private async void ExecuteAction()
    {
        var hitResult = UnityEngine.Random.Range(1, 101);
        var hitTarget = hitResult < hitProbability.value;

        //var woundTarget = WoundTest.GetWoundTest(targetNode.Unit.GetDefence(), originNode.Unit.Wargear.rangeWeapon.strength);
        var damage = WoundTest.GetWoundTest(targetNode.Unit.GetDefence(), originNode.Unit.Wargear.rangeWeapon.strength);

        // wait for animation delay in millis
        var animationDelay = 2000;
        await Task.Delay(animationDelay);

        InstantiateteProjectile(hitTarget);

        hitTargetEvent = true;

        while (hitTargetEvent)
            await Task.Yield();

        if (hitTarget)
        {
            targetNode.Unit.GetDamage(damage);
            Debug.Log($"[{originNode.Unit.name}]: Target wounded.");
        }
        else
        {
            Debug.Log($"[{originNode.Unit.name}]: Target missed.");
        }

        unitActions.FinishAction();
        this.enabled = false;
    }

    private void ShootCalculations()
    {
        var obstacles = new List<GameObject>();

        originNode.Unit.transform.LookAt(targetNode.transform.position);
        RaycastHit[] obstaclesHit = Physics.RaycastAll(originNode.Unit.transform.position + Vector3.up, originNode.Unit.transform.forward);

        foreach (var obstacle in obstaclesHit)
        {
            if (obstacle.distance < Vector3.Distance(originNode.Unit.transform.position, targetNode.Unit.transform.position))
            {
                obstacles.Add(obstacle.collider.gameObject);
            }
            obstacles.Remove(targetNode.Unit.gameObject);
        }

        // Calculating range attack chance: 100% - 15% per every obstacle on projectile's way, - 1% per every distance unit
        var hitChance = 100 - (15 * obstacles.Count) - (1 * Mathf.Round(Vector3.Distance(originNode.Unit.transform.position, targetNode.Unit.transform.position)));

        hitProbability.value = hitChance;
        updateProbability?.Invoke();
    }

    private void InstantiateteProjectile(bool getHit)
    {
        var offset = Vector3.up;
        var activeUnit = originNode.Unit;

        var projectile = Instantiate(activeUnit.Wargear.rangeWeapon.arrowPrefab,
            activeUnit.transform.position + offset, activeUnit.transform.rotation);

        var projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.startPoint = originNode.Unit.transform.position + offset;

        projectileComponent.targetPoint = (getHit) ? targetNode.Unit.transform.position + offset :
            targetNode.Unit.transform.position + activeUnit.transform.forward * UnityEngine.Random.Range(-3, 10);
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