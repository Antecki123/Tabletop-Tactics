using UnityEngine;
using UnityEngine.AI;

public class UnitAnimations : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [Header("Animations")]
    private bool walk = false;
    private bool shoot = false;

    private void LateUpdate()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            walk = true;
        else walk = false;

        animator.SetBool("walk", walk);
    }

}
