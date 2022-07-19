using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private UnitActions unitActions;
    [SerializeField] private UnitsQueue unitsQueue;

    [Header("UI References")]
    [SerializeField] private GameEvent OnNewTurn;
    [SerializeField] private FloatVariable turnsCounter;

    private void OnEnable()
    {
        UnitActions.OnFinishAction += OverCondition;
        UnitsQueue.OnTurnEnd += NextTurn;
    }
    private void OnDisable()
    {
        UnitActions.OnFinishAction -= OverCondition;
        UnitsQueue.OnTurnEnd -= NextTurn;
    }

    private void Start()
    {
        turnsCounter.value = 0;

        unitsQueue.enabled = true;
        unitActions.enabled = true;

        NextTurn();
    }

    private void NextTurn()
    {
        turnsCounter.value++;
        OnNewTurn.Invoke();
    }

    private void OverCondition(Unit unit)
    {
        //unitActions.enabled = false;
        //unitsQueue.enabled = false;
    }
}