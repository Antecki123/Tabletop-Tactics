using System.Collections;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Unit unit;
    public bool isOccupied;

    public Vector2Int position;
    [Space]
    public int movementValue = 0;

    private LineRenderer boarder;
    private readonly int unitsLayer = 512;

    private void Start()
    {
        boarder = GetComponent<LineRenderer>();

        StartCoroutine(CheckObjectOnEveryHex());        //temporary
    }

    private IEnumerator CheckObjectOnEveryHex()
    {
        yield return new WaitForEndOfFrame();

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 10f, unitsLayer) && hit.collider.GetComponent<Unit>())
        {
            unit = hit.collider.GetComponent<Unit>();
            isOccupied = true;
        }
    }

}