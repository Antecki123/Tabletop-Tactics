using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private InputsManager inputs;

    [Header("Camera Settings")]
    [SerializeField, Range(1, 30)] private float panSpeed = 50f;
    [SerializeField, Range(1, 30)] private float scrollSpeed = 50f;
    [Space]
    [SerializeField] private Vector2 mapBoundaryX;
    [SerializeField] private Vector2 mapBoundaryY;
    [SerializeField] private Vector2 mapBoundaryZ;

    private void Start() => StartCoroutine(InitialSettings());

    private IEnumerator InitialSettings()
    {
        yield return new WaitForEndOfFrame();

        mapBoundaryX.x = 0.0f;
        mapBoundaryX.y = gridManager.GridDimensions.x;

        mapBoundaryZ.x = -5.0f;
        mapBoundaryZ.y = gridManager.GridDimensions.y * .866f - 7.0f;
    }

    private void Update()
    {
        Vector3 position = transform.position;

        position.x += panSpeed * inputs.HorizontalMovement * Time.deltaTime;
        position.z += panSpeed * inputs.VerticalMovement * Time.deltaTime;

        position.y -= scrollSpeed * inputs.Scroll * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, min: mapBoundaryX.x, max: mapBoundaryX.y);
        position.y = Mathf.Clamp(position.y, min: mapBoundaryY.x, max: mapBoundaryY.y);
        position.z = Mathf.Clamp(position.z, min: mapBoundaryZ.x, max: mapBoundaryZ.y);

        transform.position = position;
    }
}