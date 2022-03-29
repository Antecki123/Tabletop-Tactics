using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;
    public enum Player { Player1, Player2 }
    public enum Phase { Priority, Move, Actions, End }

    [Header("Component References")]
    [SerializeField] private Phase activePhase = Phase.Priority;

    [SerializeField] private PhaseMovement phaseMovement;
    [SerializeField] private PhaseShooting phaseShooting;

    [Header("Gameplay References")]
    public Player activePlayer;
    [Space]
    [SerializeField] private List<Unit> player1Units = new List<Unit>();
    [SerializeField] private List<Unit> player2Units = new List<Unit>();

    [SerializeField] private bool player1TurnDone;
    [SerializeField] private bool player2TurnDone;

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
            case Phase.Move:
                MovePhase();
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
        var playerPriorityRoll = Random.Range(0, 2);

        if (playerPriorityRoll == 0)
            activePlayer = Player.Player1;
        else if (playerPriorityRoll == 1)
            activePlayer = Player.Player2;

        print($"Roll: {playerPriorityRoll}");

        activePhase = Phase.Move;
    }

    private void MovePhase()
    {
        phaseMovement.enabled = true;
        phaseShooting.enabled = false;


    }

    private void ActionsPhase()
    {
        phaseMovement.enabled = false;
        phaseShooting.enabled = true;


    }

    private void EndPhase()
    {
        phaseMovement.enabled = false;
        phaseShooting.enabled = false;

        foreach (var unit in player1Units)
        {
            unit.moveLeft = unit.unitMove;
            unit.shootAvailable = true;
            unit.duelAvailable = true;
        }
        foreach (var unit in player2Units)
        {
            unit.moveLeft = unit.unitMove;
            unit.shootAvailable = true;
            unit.duelAvailable = true;
        }
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
                activePhase = Phase.Move;
            else if (activePhase == Phase.Move)
                activePhase = Phase.Actions;
            else if (activePhase == Phase.Actions)
                activePhase = Phase.End;
            else if (activePhase == Phase.End)
                activePhase = Phase.Priority;
        }
    }
}