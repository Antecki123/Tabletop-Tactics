using UnityEngine;

public class ActiveUnitMarker : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private QueueBehavior queue;
    [SerializeField] private UnitActions actions;
    [Space]
    [SerializeField] private GameObject unitMarker;

    public void Update()
    {
        if (queue.UnitsQueue.Count != 0 && actions.State != UnitActions.UnitState.ExecutingAction)
        {
            unitMarker.transform.position = queue.UnitsQueue[0].transform.position + Vector3.up * .2f;
            unitMarker.SetActive(true);
        }
        else
        {
            unitMarker.SetActive(false);
            unitMarker.transform.position = Vector3.zero;
        }
    }
}