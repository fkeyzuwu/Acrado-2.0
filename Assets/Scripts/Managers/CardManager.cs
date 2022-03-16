using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

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

        if(deck.CardDeck.Count < amount)
        {
            Debug.Log("can't draw more cards when deck is empty/gonna be empty");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab);
            NetworkServer.Spawn(cardObject, sender);
            string cardName = deck.CardDeck.Pop();
            player.RpcShowCard(cardObject, cardName, CardState.Hand);
            cardObject.GetComponent<CardData>().InitializeCard(cardName, player.MyID);
            MatchDatabase.instance.AddCardToHand(playerID, cardObject.GetComponent<CardData>());
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayCard(GameObject cardObject, NetworkConnectionToClient sender = null)
    {
        PlayerView player = sender.identity.GetComponent<PlayerView>();
        CardData card = cardObject.GetComponent<CardData>();
        player.RpcShowCard(cardObject, cardObject.name, CardState.Board);

        if(player.MyGameState == GameState.Player1Turn)
        {
            if((gameManager.player1CurrentMana - card.currentMana) >= 0)
            {
                gameManager.player1CurrentMana -= card.currentMana;
            }
        }
        else
        {
            if((gameManager.player2CurrentMana - card.currentMana) >= 0)
            {
                gameManager.player2CurrentMana -= card.currentMana;
            }
        }

        MatchDatabase.instance.RemoveCardFromHand(player.MyID, cardObject.GetComponent<CardData>());
        MatchDatabase.instance.AddCardToBoard(player.MyID, cardObject.GetComponent<CardData>());
    }
    [Command(requiresAuthority = false)]
    public void CmdAttackCard(CardData attacker, CardData defender)
    {
        Debug.Log($"{attacker.card.name} attacked {defender.card.name}!");
        defender.currentHealth -= attacker.currentAttack;
        attacker.currentHealth -= defender.currentAttack;

        attacker.AttacksLeft--;

        if (defender.currentHealth <= 0)
        {
            NetworkServer.Destroy(defender.gameObject);
            MatchDatabase.instance.RemoveCardFromBoard(defender.ownerID, defender);
            MatchDatabase.instance.AddCardToGraveyard(defender.ownerID, defender);
            Debug.Log($"{defender.card.name} died!");
        }

        if(attacker.currentHealth <= 0)
        {
            NetworkServer.Destroy(attacker.gameObject);
            MatchDatabase.instance.RemoveCardFromBoard(attacker.ownerID, attacker);
            MatchDatabase.instance.AddCardToGraveyard(attacker.ownerID, attacker);
            Debug.Log($"{attacker.card.name} died!");
        }
    }

    [ClientRpc]
    public void RpcDrawCards(int amount)
    {
        PlayerView player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        player.DrawCard(amount);
    }

    [TargetRpc]
    public void TargetDrawCards(NetworkConnection target, int amount)
    {
        PlayerView player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        player.DrawCard(amount);
    }
}