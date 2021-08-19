using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateClientTurn))]
    private GameState gameState = GameState.Setup;

    [SyncVar] private int player1Mana = 0;
    [SyncVar] private int player2Mana = 0;

    public void StartGame()
    {
        gameState = GameState.Player1Turn;
    }

    public void NextTurn()
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

    public void UpdateClientTurn(GameState oldGameState, GameState newGameState)
    {
        if(newGameState == GameState.Player2Turn)
        {
            if(player2Mana < 10)
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
