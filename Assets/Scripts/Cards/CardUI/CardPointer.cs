using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class CardPointer : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDrawing;
    [SerializeField] private CardData cardData;

    void Update()
    {
        if (!hasAuthority) return;

        if (cardData.card.state != CardState.Board) return;

        if (isDrawing)
        {
            Debug.DrawLine(transform.position, Input.mousePosition);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
        Debug.DrawLine(transform.position, transform.position); // no line
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
        if(hit.collider.gameObject.TryGetComponent(out CardData enemyCardData))
        {
            cardData.card.Health -= enemyCardData.card.Attack;
            enemyCardData.card.Health -= cardData.card.Attack;
        }
    }
}
