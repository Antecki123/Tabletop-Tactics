using System.Collections;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Unit unit;
    public bool isOccupied;

    private void Start() => StartCoroutine(CheckObjectOnEveryHex());

    private IEnumerator CheckObjectOnEveryHex()
    {
        yield return new WaitForEndOfFrame();

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 10f, 512) && hit.collider.GetComponent<Unit>())
        {
            unit = hit.collider.GetComponent<Unit>();
            isOccupied = true;
        }
    }

    private void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }
    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = new Color32(0, 135, 0, 255);
    }
}