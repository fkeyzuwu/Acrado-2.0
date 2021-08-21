using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deck : NetworkBehaviour
{
    private Stack<string> cardDeck = new Stack<string>();
    [SerializeField] private Card[] cardArray;

    void Start()
    {
        CreateDeck();
    }

    public void CreateDeck()
    {
        int n = cardArray.Length;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int r = i + (int)(Random.Range(0f, 1f) * (n - i));
            Card card = cardArray[r];
            cardArray[r] = cardArray[i];
            cardArray[i] = card;
        }

        for (int i = 0; i < n; i++)
        {
            cardDeck.Push(cardArray[i].name);
        }
    }

    public Stack<string> CardDeck
    {
        get { return cardDeck; }
        set { cardDeck = value; }
    }
}
