using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArcMarker : MonoBehaviour, IVisualMarker
{
    [Header("Component References")]
    [SerializeField] private LineRenderer line;

    [Header("Range Action Arc Attributes")]
    [SerializeField, Range(1, 100)] private int resolution = 20;

    private GridCell originNode;
    private GridCell targetNode;

    public void TurnOnMarker(GridCell origin, GridCell target)
    {
        var pointList = new List<Vector3>();
        originNode = origin;
        targetNode = target;

        if (!origin.Equals(target))
        {
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / resolution)
            {
                var tangentLineVertex1 = Vector3.Lerp(originNode.transform.position, MiddlePosition(), ratio);
                var tangentLineVertex2 = Vector3.Lerp(MiddlePosition(), targetNode.transform.position, ratio);

                var bazierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                pointList.Add(bazierPoint);
            }

            line.enabled = true;
            line.positionCount = pointList.Count;
            line.SetPositions(pointList.ToArray());
        }
        else
            TurnOffMarker();
    }

    private Vector3 MiddlePosition()
    {
        var posX = ((targetNode.transform.position.x - originNode.transform.position.x) / 2) + originNode.transform.position.x;
        var posY = Vector3.Distance(originNode.transform.position, targetNode.transform.position) / 3;
        var posZ = ((targetNode.transform.position.z - originNode.transform.position.z) / 2) + originNode.transform.position.z;

        return new Vector3(posX, posY, posZ);
    }

    public void TurnOffMarker()
    {
        line.enabled = false;
    }
}