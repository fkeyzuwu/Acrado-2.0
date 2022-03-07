using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class CardAttack : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CardData cardData;
    private PlayerView playerView;
    private bool isDragging = false;
    private bool isControllable = true;
    private GameObject target;

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

    void Update()
    {
        if (isDragging)
        {
            if (target == null)
            {
                //draw just the arrow no target
            }
            else
            {
                //draw with the circle on where you are currently hovering
            }
        }
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

            if (cardData.card.state != CardState.Board)
            {
                Debug.Log($"Card state is not board, it is {cardData.card.state}");
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

        Debug.Log("Attacking");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanAttack) return;
        
        if (isDragging == false) return;

        isDragging = false;

        if(target != null)
        {
            playerView.AttackCard(cardData, target.GetComponent<CardData>());
            Debug.Log($"Attacking {target.GetComponent<CardData>().card.name}");
        }
        else
        {
            Debug.Log("no target");
        }
    }

    /// <summary>
    /// figure out how to get the card that my mouse is moving on and stuff
    /// </summary>
    /// <param name="collision"></param>

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Card"))
        {
            target = collision.gameObject;
            Debug.Log($"Current target - {target.GetComponent<CardData>().card.name}");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Card"))
        {
            target = null;
            Debug.Log("Currently isnt hovering over any target");
        }
    }
}
