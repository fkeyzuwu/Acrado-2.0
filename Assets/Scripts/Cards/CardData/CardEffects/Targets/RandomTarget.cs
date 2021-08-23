using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : Target
{
    private Transform playerArea;
    private Transform enemyArea;
    protected override void SetTarget()
    {
        playerArea = GameObject.Find("PlayerArea").transform;
        enemyArea = GameObject.Find("EnemyArea").transform;

        List<Transform> cards = new List<Transform>();

        foreach(Transform child in playerArea)
        {
            cards.Add(child);
        }

        foreach(Transform child in enemyArea)
        {
            cards.Add(child);
        }

        targetCard = cards[Random.Range(0, cards.Count - 1)].gameObject;
    }
}
