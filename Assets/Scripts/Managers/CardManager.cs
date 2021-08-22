using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardManager : NetworkBehaviour
{
    private GameManager gameManager;

    private Deck player1Deck;
    private Deck player2Deck;

    [SerializeField] GameObject cardPrefab;

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public override void OnStartServer()
    {
        player1Deck = GameObject.Find("Player1Deck").GetComponent<Deck>();
        player2Deck = GameObject.Find("Player2Deck").GetComponent<Deck>();
    }

    [Command(requiresAuthority = false)]
    public void CmdDrawCards(int amount, int playerID, NetworkConnectionToClient sender = null)
    {
        Deck deck = null;
        PlayerView player = sender.identity.GetComponent<PlayerView>();

        if (playerID == 1)
        {
            deck = player1Deck;
        }
        else if (playerID == 2)
        {
            deck = player2Deck;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab);
            NetworkServer.Spawn(cardObject, sender);
            string cardName = deck.CardDeck.Pop();
            player.RpcShowCard(cardObject, cardName, CardState.Hand);
            cardObject.GetComponent<CardData>().InitializeCard(cardName);
        }
    }

    [Command]
    public void CmdPlayCard(GameObject cardObject)
    {

    }

    [ClientRpc]
    public void RpcDrawCards(int amount)
    {
        PlayerView player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        player.DrawCard(amount);
    }
}