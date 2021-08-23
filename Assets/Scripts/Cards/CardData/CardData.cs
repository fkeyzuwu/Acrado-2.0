using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardData : NetworkBehaviour
{
    private string path = "ScriptableCards/";

    [SerializeField] private CardDisplay cardDisplay;

    public Card card;

    public void InitializeCard(string cardName)
    {
        card = Resources.Load<Card>(path + cardName);

        card.state = CardState.Hand;

        cardDisplay.InitializeCard(card);
    }
}
