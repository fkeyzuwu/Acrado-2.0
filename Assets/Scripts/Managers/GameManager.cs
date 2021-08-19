using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject endTurnButton;

    private int playersReady = 0;

    [SyncVar(hook = nameof(UpdateClientTurn))]
    private GameState gameState = GameState.Setup;

    [SyncVar] private int player1Mana = 0;
    [SyncVar] private int player2Mana = 0;

    [Command(requiresAuthority = false)]
    public void CmdIsPlayerReady(bool isReady)
    {
        if (isReady)
        {
            playersReady++;
        }
        else
        {
            playersReady--;
        }

        if(playersReady == 2)
        {
            StartGame();
        }
    }

    [Server]
    private void StartGame()
    {
        NetworkServer.UnSpawn(GameObject.Find("ReadyButton"));
        NetworkServer.Spawn(endTurnButton, connectionToClient);
        gameState = GameState.Player1Turn;
        Debug.Log("Game Started!");
    }

    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        if(gameState == GameState.Player1Turn)
        {
            gameState = GameState.Player2Turn;
        }
        else
        {
            gameState = GameState.Player1Turn;
        }
    }

    [Server]
    private void UpdateClientTurn(GameState oldGameState, GameState newGameState)
    {
        if (newGameState == GameState.Player2Turn)
        {
            if (player2Mana < 10)
            {
                player2Mana++;
            }
        }
        else
        {
            if (player1Mana < 10)
            {
                player1Mana++;
            }
        }
    }
}
