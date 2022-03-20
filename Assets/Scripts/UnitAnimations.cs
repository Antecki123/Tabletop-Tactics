using UnityEngine;
using UnityEngine.AI;

public class UnitAnimations : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [Header("Animations")]
    private bool move = false;
    private bool shoot = false;

    private void LateUpdate()
    {
        if (navMeshAgent.hasPath)
            move = true;
        else move = false;


        animator.SetBool("move", move);
    }

}
