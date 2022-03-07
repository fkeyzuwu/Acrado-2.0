using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class CardAttack : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CardData cardData;
    [SerializeField] private AttackArrow attackArrow;
    private PlayerView playerView;
    private bool isDragging = false;
    private bool isControllable = true;

    void Start()
    {
        if (!hasAuthority)
        {
            isControllable = false;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerView = NetworkClient.connection.identity.GetComponent<PlayerView>();
    }

    public bool CanAttack
    {
        get 
        {
            if (!isControllable)
            {
                Debug.Log($"Card {cardData.card.name} is not your card");
                return false;
            }

            if (!playerView.IsMyTurn)
            {
                Debug.Log("it is not your turn");
                return false;
            }

            if (cardData.state != CardState.Board)
            {
                Debug.Log($"Card state is not board, it is {cardData.state}");
                return false;
            }

            if (cardData.AttacksLeft < 1)
            {
                Debug.Log($"Card has {cardData.AttacksLeft} attacks left");
                return false;
            }

            return true;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanAttack) return;

        isDragging = true;

        attackArrow.gameObject.SetActive(true);

        Debug.Log("Attacking");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanAttack) return;
        
        if (isDragging == false) return;

        isDragging = false;

        if (attackArrow.target != null)
        {
            playerView.AttackCard(cardData, attackArrow.target.GetComponent<CardData>());
            Debug.Log($"Attacked {attackArrow.target.GetComponent<CardData>().card.name}");
        }
        else
        {
            Debug.Log("no target");
        }

        attackArrow.gameObject.SetActive(false);
    }
}
