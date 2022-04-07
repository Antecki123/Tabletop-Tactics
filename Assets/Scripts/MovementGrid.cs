using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private Material nodeMaterial;
    [Space]
    [SerializeField] private int gridMask;
    [SerializeField] private GameObject hexPrefab;

    private void Start() => GenerateGrid();

    private void GenerateGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                var offsetX = (j % 2 == 0) ? i : i + .5f;
                var offsetY = .75f * j;

                var position = new Vector3(offsetX, 0f, offsetY);
                var hex = Instantiate(hexPrefab, position, Quaternion.Euler(90f, 0f, 0f));
                hex.transform.SetParent(this.transform);
                hex.name = $"Hex {i} {j}";

                var line = hex.AddComponent<LineRenderer>();
                line.startWidth = .025f;
                line.loop = true;
                line.positionCount = 6;
                line.material = nodeMaterial;

                line.SetPosition(0, new Vector3(0f + offsetX, 0f, .5f + offsetY) + transform.position);
                line.SetPosition(1, new Vector3(.5f + offsetX, 0f, .25f + offsetY) + transform.position);
                line.SetPosition(2, new Vector3(.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(3, new Vector3(0f + offsetX, 0f, -.5f + offsetY) + transform.position);
                line.SetPosition(4, new Vector3(-.5f + offsetX, 0f, -.25f + offsetY) + transform.position);
                line.SetPosition(5, new Vector3(-.5f + offsetX, 0f, .25f + offsetY) + transform.position);
            }
        }
    }
}