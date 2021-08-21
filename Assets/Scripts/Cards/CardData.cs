using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardData : NetworkBehaviour
{
    private string path = "ScriptableCards/";

    [SerializeField] private CardDisplay cardDisplay;

    public Card card;
    public CardState state = CardState.Uninitialized;

    public void InitializeCard(string cardName)
    {
        if (card != null)
        {
            Debug.Log("Card already intialized");
            return;
        }

        card = Resources.Load<Card>(path + cardName);

        if (card == null)
        {
            Debug.LogError("Can't initiallize prefab when card is null");
            return;
        }

        card.state = CardState.Hand;

        cardDisplay.InitializeCard(card);
    }


    public Card Card
    {
        get
        {
            if (hasAuthority)
            {
                return card;
            }
            else if (card.state == CardState.Board)
            {
                return card;
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (hasAuthority)
            {
                card = value;
            }
        }
    }
}
