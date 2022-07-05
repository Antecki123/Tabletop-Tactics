using UnityEngine;

public class ActiveUnitMarker : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private UnitsQueue queue;
    [SerializeField] private UnitActions actions;
    [Space]
    [SerializeField] private GameObject unitMarker;

    public void Update()
    {
        if (queue.UnitsList.Count != 0 && actions.State != UnitActions.UnitState.ExecutingAction)
        {
            unitMarker.transform.position = queue.ActiveUnit.transform.position + Vector3.up * .2f;
            unitMarker.SetActive(true);
        }
        else
        {
            unitMarker.SetActive(false);
            unitMarker.transform.position = Vector3.zero;
        }
    }
}