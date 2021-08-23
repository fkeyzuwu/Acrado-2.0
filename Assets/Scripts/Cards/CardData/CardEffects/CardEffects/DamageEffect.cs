using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : CardEffect
{
    public int damage;
    public override void Activate()
    {
        Card card = target.targetCard.GetComponent<CardData>().card;
        card.health -= damage;

        if (card.health <= 0)
        {
            Object.Destroy(target.targetCard);
        }
    }
}
