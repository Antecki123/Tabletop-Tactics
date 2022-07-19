using System.Threading.Tasks;
using UnityEngine;

public class ActiveUnitMarker : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private UnitsQueue queue;
    [SerializeField] private UnitActions actions;
    [Space]
    [SerializeField] private GameObject unitMarker;

    //private void OnEnable() => UnitActions.OnFinishAction += SetMarker;
    //private void OnDisable() => UnitActions.OnFinishAction -= SetMarker;

    /*private async void SetMarker(Unit unit)
    {
        while (queue.ActiveUnit == unit)
            await Task.Yield();

        if (queue.UnitsOrder.Count != 0 && actions.State != UnitActions.UnitState.ExecutingAction)
        {
            unitMarker.transform.position = queue.ActiveUnit.transform.position + Vector3.up * .2f;
            unitMarker.SetActive(true);
        }
        else
        {
            unitMarker.SetActive(false);
            unitMarker.transform.position = Vector3.zero;
        }
    }*/

    private void Update()
    {
        if (queue.UnitsOrder.Count != 0 && actions.State != UnitActions.UnitState.ExecutingAction)
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