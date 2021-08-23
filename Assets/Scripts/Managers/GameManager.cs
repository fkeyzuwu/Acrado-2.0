using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    private CardManager cardManager;
    private TextMeshProUGUI playerMana;
    private TextMeshProUGUI enemyMana;

    private Button endTurnButton;

    [SerializeField] private int playersReady = 0;

    [SyncVar(hook = nameof(UpdateClientTurn))]
    public GameState gameState = GameState.Setup;

    [SyncVar] public int player1ID = 1;
    [SyncVar] public int player2ID = 2;

    [SyncVar(hook = nameof(UpdateManaUI))]
    public int player1MaxMana = 0;

    [SyncVar(hook = nameof(UpdateManaUI))]
    public int player2MaxMana = 0;

    [SyncVar(hook = nameof(UpdateManaUI))]
    public int player1CurrentMana = 0;

    [SyncVar(hook = nameof(UpdateManaUI))]
    public int player2CurrentMana = 0;

    [SyncVar] public int whosTurn = 0;

    void Start()
    {
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        playerMana = GameObject.Find("PlayerMana").GetComponentInChildren<TextMeshProUGUI>();
        enemyMana = GameObject.Find("EnemyMana").GetComponentInChildren<TextMeshProUGUI>();
    }

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
        DrawCardForPlayerOnTurnStart();
        cardManager.RpcDrawCards(5);
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
        DrawCardForPlayerOnTurnStart();
    }

    [Server]
    private void DrawCardForPlayerOnTurnStart()
    {
        int playerID = gameState == GameState.Player1Turn ? 1 : 2;
        NetworkConnectionToClient connection = NetworkServer.connections[playerID];
        cardManager.TargetDrawCards(connection, 1);
    }

    [Server]
    private void UpdateMana()
    {
        if (gameState == GameState.Player1Turn)
        {
            if (player1MaxMana < 10)
            {
                player1MaxMana++;
            }
            player1CurrentMana = player1MaxMana;
        }
        else
        {
            if (player2MaxMana < 10)
            {
                player2MaxMana++;
            }
            player2CurrentMana = player2MaxMana;
        }
    }

    private void UpdateClientTurn(GameState _, GameState newGameState)
    {
        PlayerView player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        player.IsMyTurn = player.MyGameState == gameState;
        endTurnButton.interactable = player.IsMyTurn;
        endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = player.IsMyTurn ? "End Turn" : "Enemy Turn";
    }

    [ClientRpc]
    private void RpcUnlockButtons()
    {
        GameObject buttonObject = GameObject.Find("EndTurnButton");
        endTurnButton = buttonObject.GetComponent<Button>();
    }

    private void UpdateManaUI(int _, int newValue)
    {
        if(NetworkClient.connection.identity.GetComponent<PlayerView>().MyID == 1)
        {
            playerMana.text = $"{player1CurrentMana} / {player1MaxMana}";
            enemyMana.text = $"{player2CurrentMana} / {player2MaxMana}";
        }
        else if(NetworkClient.connection.identity.GetComponent<PlayerView>().MyID == 2)
        {
            playerMana.text = $"{player2CurrentMana} / {player2MaxMana}";
            enemyMana.text = $"{player1CurrentMana} / {player1MaxMana}";
        }
        else
        {
            Debug.Log("uhhhh wtf");
        }
    }
}