using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardManager : NetworkBehaviour
{
    private GameManager gameManager;

    private Transform playerHand;
    private Transform playerBoard;

    private Transform enemyHand;
    private Transform enemyBoard;


    [SerializeField] GameObject cardPrefab;

    protected virtual void Start()
    {
        playerHand = GameObject.Find("PlayerHand").transform;
        playerBoard = GameObject.Find("PlayerBoard").transform;
        enemyHand = GameObject.Find("EnemyHand").transform;
        enemyBoard = GameObject.Find("EnemyBoard").transform;
    }

    [Command]
    public void CmdDrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, playerHand);
            NetworkServer.Spawn(cardObject, connectionToClient);
            RpcShowCard(cardObject, "get this from deck", CardState.Hand);
        }
    }

    [ClientRpc]
    public void RpcShowCard(GameObject cardObject, string cardName, CardState cardState)
    {
        if (hasAuthority)
        {
            cardObject.GetComponent<CardDisplay>().InitializeCard(cardName);
        }
        else if (cardState == CardState.Board)
        {
            cardObject.GetComponent<CardFlipper>().Flip();
            cardObject.GetComponent<CardDisplay>().InitializeCard(cardName);
        }
        else
        {
            cardObject.GetComponent<CardFlipper>().Flip();
        }
    }

    [Server]
    private Deck GetDeck(int playerID)
    {
        Deck deck = new Deck();
        return deck;
    }
}