using UnityEngine;

public class VisualEfects : MonoBehaviour
{
    public static VisualEfects Instance;

    [Header("Markers References")]
    [SerializeField] private GameObject arcMarker;
    [SerializeField] private GameObject movementMarker;
    [SerializeField] private GameObject positionMarker;
    [Space]
    [Header("Grid Highlights")]
    [SerializeField] private GameObject gridHighlight;

    #region Properties
    public IVisualMarker ArcMarker { get => arcMarker.GetComponent<IVisualMarker>(); }
    public IVisualMarker MovementMarker { get => movementMarker.GetComponent<IVisualMarker>(); }
    public IVisualMarker PositionMarker { get => positionMarker.GetComponent<IVisualMarker>(); }

    public IGridHighlight GridHighlight { get => gridHighlight.GetComponent<IGridHighlight>(); }
    #endregion

    private void Awake()
    {
        if (!Instance)
            Instance = this;
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
    public void TurnOnHighlightSimpleRange(GridCell startNode, int range);
    public void TurnOnHighlightMovement(GridCell startNode, int range, int actions);
    public void TurnOffHighlight();
}