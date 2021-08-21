using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : NetworkBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    public void InitializeCard(Card card)
    {
        gameObject.name = card.name;
        image.sprite = card.sprite;
        manaCostText.text = card.manaCost.ToString();
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
    }
}
