using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PositionArcRenderer : MonoBehaviour
{
    [Header("Range Action Arc Attributes")]
    [SerializeField, Range(1, 100)] private int resolution;
    [SerializeField] private GameObject positionMarker;

    private LineRenderer line;

    private GridCell originNode;
    private GridCell targetNode;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        RangeAttack.OnFindingTarget += CalculateArc;
        RangeAttack.OnFindingTarget += SetMarkerPosition;
        RangeAttack.OnClearAction += TurnOff;

        MeleeAttack.OnFindingTarget += CalculateArc;
        MeleeAttack.OnFindingTarget += SetMarkerPosition;
        MeleeAttack.OnClearAction += TurnOff;
    }
    private void OnDisable()
    {
        RangeAttack.OnFindingTarget -= CalculateArc;
        RangeAttack.OnFindingTarget -= SetMarkerPosition;
        RangeAttack.OnClearAction -= TurnOff;

        MeleeAttack.OnFindingTarget -= CalculateArc;
        MeleeAttack.OnFindingTarget -= SetMarkerPosition;
        MeleeAttack.OnClearAction -= TurnOff;
    }

    private void TurnOff()
    {
        line.enabled = false;
        positionMarker.SetActive(false);
    }

    private void CalculateArc(GridCell origin, GridCell target)
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
    }

    private Vector3 MiddlePosition()
    {
        var posX = ((targetNode.transform.position.x - originNode.transform.position.x) / 2) + originNode.transform.position.x;
        var posY = Vector3.Distance(originNode.transform.position, targetNode.transform.position) / 2;
        var posZ = ((targetNode.transform.position.z - originNode.transform.position.z) / 2) + originNode.transform.position.z;

        return new Vector3(posX, posY, posZ);
    }

    private void SetMarkerPosition(GridCell origin, GridCell target)
    {
        if (targetNode && !origin.Equals(target))
        {
            positionMarker.SetActive(true);
            positionMarker.transform.position = targetNode.transform.position + Vector3.up * .3f;
        }
        else TurnOff();
    }
}