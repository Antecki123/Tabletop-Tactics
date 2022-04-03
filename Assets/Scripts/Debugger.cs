using UnityEngine;

public class Debugger : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private PhaseMovement phaseMovement;
    [SerializeField] private PhaseAction phaseAction;

    [Header("Movement Debugger")]
    [SerializeField] private LineRenderer line;

    private void Awake()
    {
        mainCamera = Camera.main;

        phaseManager = FindObjectOfType<PhaseManager>();
        phaseMovement = FindObjectOfType<PhaseMovement>();
        phaseAction = FindObjectOfType<PhaseAction>();

        line = GetComponent<LineRenderer>();
        line.startWidth = .1f;
        line.startColor = Color.red;
    }

    private void OnDrawGizmos()
    {
        // MOVEMENT
        if (Application.isPlaying && phaseManager.activePhase == PhaseManager.Phase.Move && phaseMovement.ActiveUnit)
        {
            print(phaseMovement.ActiveUnit);
            if (phaseMovement.ActiveUnit.navMeshAgent.hasPath)
            {
                line.positionCount = phaseMovement.ActiveUnit.navMeshAgent.path.corners.Length;
                line.SetPositions(phaseMovement.ActiveUnit.navMeshAgent.path.corners);
                line.enabled = true;
            }
            else line.enabled = false;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(phaseMovement.ActiveUnit.transform.position, phaseMovement.ActiveUnit.moveLeft);
            Gizmos.DrawLine(phaseMovement.ActiveUnit.transform.position, MousePosition());
        }

        // ACTIONS
        if (Application.isPlaying && phaseManager.activePhase == PhaseManager.Phase.Actions && 
            phaseAction.activeAction == PhaseAction.UnitAction.RangeAttack && phaseAction.activeUnit)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(phaseAction.activeUnit.transform.position, phaseAction.activeUnit.Wargear.rangeWeapon.range);
            Gizmos.DrawLine(phaseAction.activeUnit.transform.position, MousePosition());
        }
        if (Application.isPlaying && phaseAction.activeUnit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(phaseAction.activeUnit.transform.position, 1f);
        }
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}
