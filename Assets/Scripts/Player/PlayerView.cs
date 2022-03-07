using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

public class PlayerView : NetworkBehaviour
{
    public static HashSet<PlayerView> ActivePlayers = new HashSet<PlayerView>();
    [SyncVar] public int MyID = -1;

    private GameState myGameState;
    private bool isMyTurn;

    private GameManager gameManager;
    private CardManager cardManager;

    private Transform playerHand;
    private Transform playerBoard;

    private Transform enemyHand;
    private Transform enemyBoard;

    protected virtual void Start()
    {
        if (isServer)
        {
            ActivePlayers.Add(this);
            MyID = ActivePlayers.Count;
        }

        playerHand = GameObject.Find("PlayerHand").transform;
        playerBoard = GameObject.Find("PlayerBoard").transform;
        enemyHand = GameObject.Find("EnemyHand").transform;
        enemyBoard = GameObject.Find("EnemyBoard").transform;
    }

    protected virtual void OnDestroy()
    {
        ActivePlayers.Remove(this);
    }

    public void ReadySate(bool isReady)
    {
        GameManager.CmdIsPlayerReady(isReady);
    }

    public void DrawCard(int amount)
    {
        CardManager.CmdDrawCards(amount, MyID);
    }

    [ClientRpc]
    public void RpcShowCard(GameObject cardObject, string cardName, CardState cardState)
    {
        if (cardState == CardState.Hand)
        {
            if (hasAuthority)
            {
                cardObject.transform.SetParent(playerHand, false);
                cardObject.GetComponent<CardData>().InitializeCard(cardName);
            }
            else
            {
                cardObject.transform.SetParent(enemyHand, false);
                cardObject.GetComponent<CardFlipper>().Flip();
            }

            cardObject.GetComponent<CardData>().state = CardState.Hand;
        }
        else if (cardState == CardState.Board)
        {
            if (hasAuthority)
            {
                cardObject.transform.SetParent(playerBoard, false);
            }
            else
            {
                cardObject.transform.SetParent(enemyBoard, false);
                cardObject.GetComponent<CardData>().InitializeCard(cardName);
                cardObject.GetComponent<CardFlipper>().Flip();
            }

            cardObject.GetComponent<CardData>().state = CardState.Board;
        }
    }

    public void AttackCard(CardData attacker, CardData defender)
    {
        if (IsMyTurn)
        {
            cardManager.CmdAttackCard(attacker, defender);
        }
    }

    public void PlayCard(GameObject cardObject)
    {
        if (IsMyTurn)
        {
            cardManager.CmdPlayCard(cardObject);
        }
    }

    public bool IsMyTurn
    {
        get
        {
            if (MyGameState == gameManager.gameState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            isMyTurn = value;
        }
    }

    public GameState MyGameState
    {
        get
        {
            if(MyID == GameManager.player1ID)
            {
                return GameState.Player1Turn;
            }
            else if (MyID == GameManager.player2ID)
            {
                return GameState.Player2Turn;
            }
            else
            {
                return GameState.Setup;
            }
        }
    }

    public GameManager GameManager
    {
        get
        {
            if(gameManager == null)
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            return gameManager;
        }
    }

    public CardManager CardManager
    {
        get
        {
            if (cardManager == null)
            {
                cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
            }

            return cardManager;
        }
    }
}
