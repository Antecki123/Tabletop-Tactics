using UnityEngine;
using UnityEngine.InputSystem;

//[CreateAssetMenu(fileName = "Inputs Manager", menuName = "Scriptable Objects/Utilities/Inputs Manager")]
public class InputsManager : MonoBehaviour
{
    [field: SerializeField] public bool LeftMouseButton { get; private set; } = false;
    [field: SerializeField] public bool RightMouseButton { get; private set; } = false;

    [field: SerializeField] public float HorizontalMovement { get; private set; } = 0.0f;
    [field: SerializeField] public float VerticalMovement { get; private set; } = 0.0f;
    [field: SerializeField] public float Scroll { get; private set; } = 0.0f;

    public Vector3 MousePosition { get => Mouse.current.position.ReadValue(); }

    private InputActions inputActions;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Gameplay_Battle.HorizontalAxis.started += ctx => HorizontalMovement = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.HorizontalAxis.canceled += ctx => HorizontalMovement = 0.0f;

        inputActions.Gameplay_Battle.VerticalAxis.started += ctx => VerticalMovement = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.VerticalAxis.canceled += ctx => VerticalMovement = 0.0f;

        inputActions.Gameplay_Battle.Scroll.started += ctx => Scroll = ctx.ReadValue<float>();
        inputActions.Gameplay_Battle.Scroll.canceled += ctx => Scroll = 0.0f;

        inputActions.Gameplay_Battle.LeftMouseButton.performed += ctx => LeftMouseButton = true;
        inputActions.Gameplay_Battle.LeftMouseButton.canceled += ctx => LeftMouseButton = false;

        inputActions.Gameplay_Battle.RightMouseButton.performed += ctx => RightMouseButton = true;
        inputActions.Gameplay_Battle.RightMouseButton.canceled += ctx => RightMouseButton = false;
    }
}