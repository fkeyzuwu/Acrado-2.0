using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    private bool isFlipped = false;

    [SerializeField] private GameObject cardData;
    [SerializeField] private Image background;
    [SerializeField] private Sprite cardFront;
    [SerializeField] private Sprite cardBack;

    public void Flip()
    {
        isFlipped = !isFlipped;

        background.sprite = isFlipped ? cardBack : cardFront;
        cardData.SetActive(!isFlipped);
    }
}
