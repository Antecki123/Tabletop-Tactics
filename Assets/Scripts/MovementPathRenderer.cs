using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovementPathRenderer : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private AStarPathfinding pathfinding;
    [SerializeField] private GameObject positionMarker;

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        Movement.OnNewPath += DrawLine;
        Movement.OnNewPath += SetMarkerPosition;
        Movement.OnClearAction += TurnOff;
    }
    private void OnDisable()
    {
        Movement.OnNewPath -= DrawLine;
        Movement.OnNewPath -= SetMarkerPosition;
        Movement.OnClearAction -= TurnOff;
    }

    private void TurnOff()
    {
        line.enabled = false;
        positionMarker.SetActive(false);
    }

    private void DrawLine()
    {
        var path = pathfinding.CurrentPath.ToArray();

        if (path.Length > 0)
        {
            for (int i = 0; i < path.Length; i++)
                path[i] += Vector3.up * .3f;

            line.enabled = true;
            line.positionCount = path.Length;
            line.SetPositions(path);
        }
    }

    private void SetMarkerPosition()
    {
        if (pathfinding.CurrentPath.Count > 0)
        {
            positionMarker.SetActive(true);
            positionMarker.transform.position = pathfinding.CurrentPath[^1] + Vector3.up * .35f;
        }
    }
}