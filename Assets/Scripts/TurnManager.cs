using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Phase { Priority, Actions, End }

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private QueueBehavior queueBehavior;

    [Header("Gameplay References")]
    [SerializeField] private Phase activePhase = Phase.Priority;

    private void Update()
    {
        switch (activePhase)
        {
            case Phase.Priority:
                PriorityPhase();
                break;
            case Phase.Actions:
                ActionsPhase();
                break;
            case Phase.End:
                EndPhase();
                break;
            default:
                break;
        }
    }

    private void PriorityPhase()
    {
        unitActions.enabled = false;

        queueBehavior.CreateNewQueue(FindObjectsOfType<Unit>());

        activePhase = Phase.Actions;
    }

    private void ActionsPhase()
    {
        unitActions.enabled = true;

        if (queueBehavior.UnitsQueue.Count == 0)
            activePhase = Phase.End;
    }

    private void EndPhase()
    {
        unitActions.enabled = false;

        foreach (var unit in FindObjectsOfType<Unit>())
            unit.ResetStats();

        activePhase = Phase.Priority;
    }
}