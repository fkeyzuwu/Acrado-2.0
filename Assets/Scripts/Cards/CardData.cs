using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardData : NetworkBehaviour
{
    private string path = "ScriptableCards/";

    [SerializeField] private CardDisplay cardDisplay;
    [SyncVar] public int AttacksLeft = 1;
    // ^^each turn reset this using the matchdatabase going through all the cards in the players side of the board, reseting the attacks left to 1

    public Card card;

    public void InitializeCard(string cardName)
    {
        card = Resources.Load<Card>(path + cardName);

        card.state = CardState.Hand;

        cardDisplay.InitializeCard(card);
    }
}
