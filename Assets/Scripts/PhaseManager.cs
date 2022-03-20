using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;
    public enum Player { None, Player1, Player2 }
    public enum Phase { Priority, Move, Shoot, Fight, End }

    [Header("Component References")]
    [SerializeField] private Phase activePhase = Phase.Priority;

    [SerializeField] private PhaseMovement phaseMovement;
    [SerializeField] private PhaseShooting phaseShooting;
    [SerializeField] private PhaseFight phaseFight;

    [Header("Gameplay References")]
    public Player activePlayer = Player.None;
    [Space]
    [SerializeField] private Player playerPriority = Player.None;
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
            case Phase.Shoot:
                ShootPhase();
                break;
            case Phase.Fight:
                FightPhase();
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
        var player1Roll = RollTest.RollDiceD6();
        var player2Roll = RollTest.RollDiceD6();

        if (playerPriority == Player.None)
        {
            if (player1Roll > player2Roll)
                playerPriority = Player.Player1;
            else if (player1Roll < player2Roll)
                playerPriority = Player.Player2;
            else
                return;
        }
        else
        {
            if (player1Roll > player2Roll)
                playerPriority = Player.Player1;
            else if (player1Roll < player2Roll)
                playerPriority = Player.Player2;
            else
            {
                if (playerPriority == Player.Player1)
                    playerPriority = Player.Player2;
                if (playerPriority == Player.Player2)
                    playerPriority = Player.Player1;
            }
        }

        RollResultsPanel.instance.ShowResult(player1Roll, "Player 1");
        RollResultsPanel.instance.ShowResult(player2Roll, "Player 2");

        print($"Rolls: {player1Roll} | {player2Roll}");

        activePlayer = playerPriority;
        activePhase = Phase.Move;
    }

    private void MovePhase()
    {
        phaseMovement.enabled = true;
        phaseShooting.enabled = false;
        phaseFight.enabled = false;


    }

    private void ShootPhase()
    {
        phaseMovement.enabled = false;
        phaseShooting.enabled = true;
        phaseFight.enabled = false;


    }

    private void FightPhase()
    {
        phaseMovement.enabled = false;
        phaseShooting.enabled = false;
        phaseFight.enabled = true;


    }

    private void EndPhase()
    {
        phaseMovement.enabled = false;
        phaseShooting.enabled = false;
        phaseFight.enabled = false;

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
                activePhase = Phase.Shoot;
            else if (activePhase == Phase.Shoot)
                activePhase = Phase.Fight;
            else if (activePhase == Phase.Fight)
                activePhase = Phase.End;
            else if (activePhase == Phase.End)
                activePhase = Phase.Priority;
        }
    }
}