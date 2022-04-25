using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;
    public enum Player { Player1, Player2 }
    public enum Phase { Priority, Actions, End }

    [Header("Component References")]
    [SerializeField] private Movement phaseMovement;
    [SerializeField] private PhaseActions phaseAction;

    [Header("Gameplay References")]
    [SerializeField] private Phase activePhase = Phase.Priority;
    [SerializeField] private Player activePlayer;
    [Space]
    [SerializeField] private List<Unit> player1Units = new();
    [SerializeField] private List<Unit> player2Units = new();
    [SerializeField] private Queue<Unit> unitsQueue = new();

    [SerializeField] private bool player1TurnDone;
    [SerializeField] private bool player2TurnDone;

    public Player ActivePlayer { get => activePlayer; }

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

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
        /*
        var playerPriorityRoll = Random.Range(1, 3);

        if (playerPriorityRoll == 1)
            activePlayer = Player.Player1;
        else if (playerPriorityRoll == 2)
            activePlayer = Player.Player2;

        print($"Roll: {playerPriorityRoll}");
        */

        var allUnits = FindObjectsOfType<Unit>();

        System.Array.Sort(allUnits, (a, b) => a.unitSpeed.CompareTo(b.unitSpeed));

        foreach (var unit in allUnits)
            unitsQueue.Enqueue(unit);

        print(unitsQueue.Count);

        activePhase = Phase.Actions;
    }

    private void ActionsPhase()
    {
        phaseAction.enabled = true;

        var activeUnit = unitsQueue.Peek();
        if (activeUnit.RemainingActions > 0)
        {
            PhaseActions.instance.ActiveUnit = activeUnit;
            //unitsQueue.Dequeue();
        }

    }

    private void EndPhase()
    {
        phaseAction.enabled = false;
        player1TurnDone = true;
        player2TurnDone = true;

        foreach (var unit in player1Units)
            unit.ResetStats();

        foreach (var unit in player2Units)
            unit.ResetStats();
    }

    public void NextPhase_Btn()
    {
        if (activePlayer == Player.Player1)
        {
            activePlayer = Player.Player2;
            player1TurnDone = true;
        }
        else if (activePlayer == Player.Player2)
        {
            activePlayer = Player.Player1;
            player2TurnDone = true;
        }

        if (player1TurnDone && player2TurnDone)
        {
            player1TurnDone = false;
            player2TurnDone = false;

            if (activePhase == Phase.Priority)
                activePhase = Phase.Actions;
            else if (activePhase == Phase.Actions)
                activePhase = Phase.End;
            else if (activePhase == Phase.End)
                activePhase = Phase.Priority;
        }

        PhaseActions.instance.ClearActiveAction();
    }
}