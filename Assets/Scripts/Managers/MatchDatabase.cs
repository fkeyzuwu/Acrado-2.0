using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchDatabase : NetworkBehaviour
{
    public static MatchDatabase instance;

    private List<CardData> player1CurrentDeck = new List<CardData>();
    private List<CardData> player2CurrentDeck = new List<CardData>();
    private List<CardData> player1CurrentHand = new List<CardData>();
    private List<CardData> player2CurrentHand = new List<CardData>();
    private List<CardData> player1CurrentBoard = new List<CardData>();
    private List<CardData> player2CurrentBoard = new List<CardData>();

    public List<CardData> Player1CurrentDeck { get => player1CurrentDeck; }
    public List<CardData> Player2CurrentDeck { get => player2CurrentDeck; }
    public List<CardData> Player1CurrentHand { get => player1CurrentHand; }
    public List<CardData> Player2CurrentHand { get => player2CurrentHand; }
    public List<CardData> Player1CurrentBoard { get => player1CurrentBoard; }
    public List<CardData> Player2CurrentBoard { get => player2CurrentBoard; }

    void Start()
    {
        instance = this;
    }

    public void AddCardToHand(int playerID, CardData card)
    {
        if(playerID == 1)
        {
            player1CurrentHand.Add(card);
        }
        else
        {
            player2CurrentHand.Add(card);
        }

        Debug.Log($"Added {card.card.name} to player{playerID}'s hand");
    }

    public void RemoveCardFromHand(int playerID, CardData card)
    {
        if (playerID == 1)
        {
            player1CurrentHand.Remove(card);
        }
        else
        {
            player2CurrentHand.Remove(card);
        }

        Debug.Log($"Removed {card.card.name} From player{playerID}'s hand");
    }

    public void AddCardToBoard(int playerID, CardData card)
    {
        if (playerID == 1)
        {
            player1CurrentBoard.Add(card);
        }
        else
        {
            player2CurrentBoard.Add(card);
        }

        Debug.Log($"Added {card.card.name} to player{playerID}'s board");
    }

    public void RemoveCardFromBoard(int playerID, CardData card)
    {
        if (playerID == 1)
        {
            player1CurrentBoard.Remove(card);
        }
        else
        {
            player2CurrentBoard.Remove(card);
        }

        Debug.Log($"Removed {card.card.name} From player{playerID}'s board");
    }
}
