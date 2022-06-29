using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField, Range(1, 30)] private float panSpeed = 50f;
    [SerializeField, Range(1, 30)] private float scrollSpeed = 50f;
    [Space]
    [SerializeField] private Vector2 mapBoundaryX;
    [SerializeField] private Vector2 mapBoundaryY;
    [SerializeField] private Vector2 mapBoundaryZ;

    private float horizontalMovement;
    private float verticalMovement;
    private float scroll;

    private InputActions inputActions;
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Gameplay_Battle.HorizontalAxis.started += ctx => horizontalMovement = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.HorizontalAxis.canceled += ctx => horizontalMovement = 0.0f;

        inputActions.Gameplay_Battle.VerticalAxis.started += ctx => verticalMovement = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.VerticalAxis.canceled += ctx => verticalMovement = 0.0f;

        inputActions.Gameplay_Battle.Scroll.started += ctx => scroll = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.Scroll.canceled += ctx => scroll = 0.0f;

    }

    private void Update()
    {
        Vector3 position = transform.position;

        position.x += panSpeed * horizontalMovement * Time.deltaTime;
        position.z += panSpeed * verticalMovement * Time.deltaTime;

        position.y -= scrollSpeed * scroll * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, min: mapBoundaryX.x, max: mapBoundaryX.y);
        position.y = Mathf.Clamp(position.y, min: mapBoundaryY.x, max: mapBoundaryY.y);
        position.z = Mathf.Clamp(position.z, min: mapBoundaryZ.x, max: mapBoundaryZ.y);

        transform.position = position;
    }
}