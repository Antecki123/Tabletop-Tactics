using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UnitActions actions;
    [Space]
    [SerializeField] private TextMeshProUGUI activePlayerHUD;

    [Header("Movement Debugger")]
    [SerializeField] private LineRenderer line;

    private void Awake()
    {
        mainCamera = Camera.main;
        actions = FindObjectOfType<UnitActions>();

        line = GetComponent<LineRenderer>();
        line.startWidth = .1f;
        line.startColor = Color.red;
    }


    private void Update()
    {
        if (actions.ActiveUnit)
            activePlayerHUD.text = $"Active Unit: {actions.ActiveUnit.name}";

        /*
        var activePlayer = TurnManager.instance.ActivePlayer;
        if (activePlayer == TurnManager.Player.Player1)
            activePlayerHUD.text = "Player Red Turn";
        else if (activePlayer == TurnManager.Player.Player2)
            activePlayerHUD.text = "Player Blue Turn";
        */
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (actions.ActiveUnit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(actions.ActiveUnit.transform.position, .5f);
            }

            if (actions.ActiveAction == UnitActions.UnitAction.Movement)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(actions.ActiveUnit.transform.position, MousePosition());
            }

            else if (actions.ActiveAction == UnitActions.UnitAction.RangeAttack && actions.ActiveUnit)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(actions.ActiveUnit.transform.position, actions.ActiveUnit.Wargear.rangeWeapon.range);
                Gizmos.DrawLine(actions.ActiveUnit.transform.position, MousePosition());
            }

            else if (actions.ActiveAction == UnitActions.UnitAction.MeleeAttack && actions.ActiveUnit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(actions.ActiveUnit.transform.position, MousePosition());
            }
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}
