using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoomCardDisplay : MonoBehaviour
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Card card;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start()
    {
        card = cardData.card;
        Debug.Log(card);
        nameText.text = card.name;
        descriptionText.text = card.description;
        image.sprite = card.sprite;
        manaCostText.text = card.manaCost.ToString();
        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
    }
}
