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
        var activePlayer = PhaseManager.instance.activePlayer;
        if (activePlayer == PhaseManager.Player.Player1)
            activePlayerHUD.text = "Player Red Turn";
        else if (activePlayer == PhaseManager.Player.Player2)
            activePlayerHUD.text = "Player Blue Turn";

    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (actions.activeUnit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(actions.activeUnit.transform.position, 1f);
            }

            if (actions.activeAction == PhaseActions.UnitAction.Movement && actions.activeUnit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(actions.activeUnit.transform.position, MousePosition());

                if (actions.activeUnit.navMeshAgent.hasPath)
                {
                    line.positionCount = actions.activeUnit.navMeshAgent.path.corners.Length;
                    line.SetPositions(actions.activeUnit.navMeshAgent.path.corners);
                    line.enabled = true;
                }
                else line.enabled = false;
            }

            else if (actions.activeAction == PhaseActions.UnitAction.RangeAttack && actions.activeUnit)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(actions.activeUnit.transform.position, actions.activeUnit.RangeWeapon.range);
                Gizmos.DrawLine(actions.activeUnit.transform.position, MousePosition());
            }

            else if (actions.activeAction == PhaseActions.UnitAction.MeleeAttack && actions.activeUnit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(actions.activeUnit.transform.position, MousePosition());
            }
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}
