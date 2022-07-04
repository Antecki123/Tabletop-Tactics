using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Phase { NewTurn, Actions, End }

    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private UnitsQueue unitsQueue;
    [Space]
    [SerializeField] private GameEvent OnNewTurn;
    [SerializeField] private FloatVariable turnsCounter;

    [Header("Gameplay References")]
    [SerializeField] private Phase activePhase = Phase.NewTurn;

    private void Start()
    {
        turnsCounter.value = 0;
    }

    private void Update()
    {
        if (activePhase == Phase.NewTurn)
            NewTurn();

        else if (activePhase == Phase.Actions)
            ActionsPhase();

        else if (activePhase == Phase.End)
            EndPhase();
    }

    private void NewTurn()
    {
        // UI: turns counter++, new turn popup panel
        // new units queue, 

        turnsCounter.value++;
        OnNewTurn.Invoke();

        unitActions.enabled = false;

        unitsQueue.CreateNewQueue(FindObjectsOfType<Unit>());

        activePhase = Phase.Actions;
    }

    private void ActionsPhase()
    {
        unitActions.enabled = true;

        if (unitsQueue.UnitsList.Count == 0)
            activePhase = Phase.End;
    }

    private void EndPhase()
    {
        unitActions.enabled = false;

        foreach (var unit in FindObjectsOfType<Unit>())
            unit.ResetStats();

        activePhase = Phase.NewTurn;
    }
}