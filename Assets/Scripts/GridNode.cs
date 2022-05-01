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

        StartCoroutine(CheckObjectOnEveryHex());
    }

    private IEnumerator CheckObjectOnEveryHex()
    {
        yield return new WaitForSeconds(.5f);
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 1f, unitsLayer) && hit.collider.GetComponent<Unit>())
        {
            unit = hit.collider.GetComponent<Unit>();
            isOccupied = true;
        }

        if (Physics.Raycast(transform.position + Vector3.down * .1f, Vector3.up, out hit) && !hit.collider.GetComponent<Unit>())
        {
            GridManager.instance.GridNodes.Remove(this);
            Destroy(this.gameObject);
        }
    }

    private void OnMouseOver()
    {
        var lineComponent = GetComponent<LineRenderer>();

        if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
        {
            lineComponent.enabled = true;
            lineComponent.startColor = Color.white;
            lineComponent.endColor = Color.white;
        }
    }

    private void OnMouseExit()
    {
        var lineComponent = GetComponent<LineRenderer>();

        if (lineComponent.startColor != Color.blue && lineComponent.startColor != Color.red && lineComponent.startColor != Color.cyan)
            lineComponent.enabled = false;
    }
}