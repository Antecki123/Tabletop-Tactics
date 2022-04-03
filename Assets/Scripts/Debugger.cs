using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    }

    private Vector3 MousePosition()
    {
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        return hit.point;
    }
}
