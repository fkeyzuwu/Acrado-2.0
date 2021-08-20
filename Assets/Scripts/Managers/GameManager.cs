using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    private Button endTurnButton;

    [SerializeField] private int playersReady = 0;

    [SyncVar(hook = nameof(UpdateClientTurn))]
    public GameState gameState = GameState.Setup;

    [SyncVar] public int player1ID = 1;
    [SyncVar] public int player2ID = 2;

    [SyncVar] public int player1Mana = 0;
    [SyncVar] public int player2Mana = 0;
    [SyncVar] public int whosTurn = 0;

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

        RpcUnlockButtons();

        whosTurn = Random.Range(1, 2);
        gameState = whosTurn == 1 ? GameState.Player1Turn : GameState.Player2Turn;

        Debug.Log("Game Started!");

        UpdateMana();
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

        UpdateMana();
    }

    [Server]
    private void UpdateMana()
    {
        if (gameState == GameState.Player1Turn)
        {
            if (player1Mana < 10)
            {
                player1Mana++;
            }
        }
        else
        {
            if (player2Mana < 10)
            {
                player2Mana++;
            }
        }
    }

    private void UpdateClientTurn(GameState _, GameState newGameState)
    {
        PlayerView player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        player.IsMyTurn = player.MyGameState == gameState;
        endTurnButton.interactable = player.IsMyTurn;
    }

    [ClientRpc]
    private void RpcUnlockButtons()
    {
        GameObject buttonObject = GameObject.Find("EndTurnButton");
        endTurnButton = buttonObject.GetComponent<Button>();
    }
}
