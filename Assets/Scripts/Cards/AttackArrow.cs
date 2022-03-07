using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrow : MonoBehaviour
{
    [SerializeField] private CardAttack cardAttack;
    [HideInInspector] public GameObject target;
    void Update()
    {
        if (target == null)
        {
            //draw just the arrow no target
        }
        else
        {
            //draw with the circle on where you are currently hovering
        }

        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void OnDisable()
    {
        transform.position = transform.parent.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Card")) return;

        if (collision.GetComponent<CardData>().hasAuthority)
        {
            Debug.Log($"The card {collision.GetComponent<CardData>().card.name} is yours, you can't attack it");
            return;
        }

        if (collision.GetComponent<CardData>().state != CardState.Board)
        {
            Debug.Log($"The card {collision.GetComponent<CardData>().card.name} is not on the board, you can't attack it");
            return;
        }

        target = collision.gameObject;
        Debug.Log($"Current target - {target.GetComponent<CardData>().card.name}");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Card"))
        {
            target = null;
            Debug.Log("Currently isnt hovering over any target");
        }
    }
}
