using UnityEngine;

public class VisualEfects : MonoBehaviour
{
    public static VisualEfects instace;

    [Header("Markers References")]
    [SerializeField] private GameObject arcMarker;
    [SerializeField] private GameObject movementMarker;
    [SerializeField] private GameObject positionMarker;
    [Space]
    [Header("Grid Highlights")]
    [SerializeField] private GameObject gridMovementRange;
    [SerializeField] private GameObject gridRange;

    #region Properties
    public IVisualMarker ArcMarker { get => arcMarker.GetComponent<IVisualMarker>(); }
    public IVisualMarker MovementMarker { get => movementMarker.GetComponent<IVisualMarker>(); }
    public IVisualMarker PositionMarker { get => positionMarker.GetComponent<IVisualMarker>(); }

    public IGridHighlight GridMovementRange { get => gridMovementRange.GetComponent<IGridHighlight>(); }
    public IGridHighlight GridRange { get => gridRange.GetComponent<IGridHighlight>(); }
    #endregion

    private void Awake()
    {
        if (!instace)
            instace = this;
        else
            Destroy(gameObject);
    }
}

public interface IVisualMarker
{
    public void TurnOnMarker(GridCell origin, GridCell target);
    public void TurnOffMarker();
}

public interface IGridHighlight
{
    public void TurnOnHighlight(GridCell startNode, int range);
    public void TurnOnHighlightMovement();
    public void TurnOffHighlight();
}