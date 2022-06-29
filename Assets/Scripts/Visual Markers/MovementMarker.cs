using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementMarker : MonoBehaviour, IVisualMarker
{
    [Header("Component References")]
    [SerializeField] private LineRenderer line;

    [Header("Component References")]
    [SerializeField] private AStarPathfinding pathfinding;

    public void TurnOnMarker(GridCell origin, GridCell target)
    {
        var path = pathfinding.CurrentPath.ToArray();

        if (path.Length > 0)
        {
            for (int i = 0; i < path.Length; i++)
                path[i] += Vector3.up * .1f;

            line.enabled = true;
            line.positionCount = path.Length;
            line.SetPositions(path);
        }
    }

    public void TurnOffMarker()
    {
        line.enabled = false;
    }
}