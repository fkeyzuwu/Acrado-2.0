using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : CardEffect
{
    public int heal;
    public override void Activate()
    {
        target.targetCard.GetComponent<CardData>().card.Health += heal;
    }
}
