using UnityEngine;

public class PositionMarker : MonoBehaviour, IVisualMarker
{
    [Header("Position Marker Attributes")]
    [SerializeField, Min(0)] private float yPosition = .2f;
    [Space]
    [SerializeField] private GameObject targetMarkerBlue;
    [SerializeField] private GameObject targetMarkerRed;

    public void TurnOnMarker(GridCell origin, GridCell target)
    {
        if (target && !origin.Equals(target))
        {
            if (target.Unit && target.Unit.UnitOwner != origin.Unit.UnitOwner)
            {
                targetMarkerBlue.SetActive(false);
                targetMarkerRed.SetActive(true);

                targetMarkerRed.transform.position = target.transform.position + Vector3.up * yPosition;
                return;
            }

            targetMarkerBlue.SetActive(true);
            targetMarkerRed.SetActive(false);

            targetMarkerBlue.transform.position = target.transform.position + Vector3.up * yPosition;
        }
        else TurnOffMarker();
    }

    public void TurnOffMarker()
    {
        targetMarkerBlue.SetActive(false);
        targetMarkerRed.SetActive(false);
    }
}