using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : NetworkBehaviour
{
    private string path = "ScriptableCards/";

    [SerializeField] private Card card;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

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

        if (hasAuthority)
        {
            gameObject.name = card.name;
            image.sprite = card.sprite;
            manaCostText.text = card.manaCost.ToString();
            attackText.text = card.attack.ToString();
            healthText.text = card.health.ToString();
        }
    }

    public Card Card
    {
        get
        {
            if (hasAuthority) 
            { 
                return card; 
            } 
            else if(card.state == CardState.Board)
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
