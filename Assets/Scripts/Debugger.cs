using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private PhaseActions actions;
    [Space]
    [SerializeField] private TextMeshProUGUI activePlayerHUD;

    [Header("Movement Debugger")]
    [SerializeField] private LineRenderer line;

    private void Awake()
    {
        mainCamera = Camera.main;

        phaseManager = FindObjectOfType<PhaseManager>();
        actions = FindObjectOfType<PhaseActions>();

        line = GetComponent<LineRenderer>();
        line.startWidth = .1f;
        line.startColor = Color.red;
    }

    private void Update()
    {
        var activePlayer = PhaseManager.instance.ActivePlayer;
        if (activePlayer == PhaseManager.Player.Player1)
            activePlayerHUD.text = "Player Red Turn";
        else if (activePlayer == PhaseManager.Player.Player2)
            activePlayerHUD.text = "Player Blue Turn";

    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (actions.ActiveUnit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(actions.ActiveUnit.transform.position, 1f);
            }

            if (actions.ActiveAction == PhaseActions.UnitAction.Movement && actions.ActiveUnit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(actions.ActiveUnit.transform.position, MousePosition());

                if (actions.ActiveUnit.navMeshAgent.hasPath)
                {
                    line.positionCount = actions.ActiveUnit.navMeshAgent.path.corners.Length;
                    line.SetPositions(actions.ActiveUnit.navMeshAgent.path.corners);
                    line.enabled = true;
                }
                else line.enabled = false;
            }

            else if (actions.ActiveAction == PhaseActions.UnitAction.RangeAttack && actions.ActiveUnit)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(actions.ActiveUnit.transform.position, actions.ActiveUnit.RangeWeapon.range);
                Gizmos.DrawLine(actions.ActiveUnit.transform.position, MousePosition());
            }

            else if (actions.ActiveAction == PhaseActions.UnitAction.MeleeAttack && actions.ActiveUnit)
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
