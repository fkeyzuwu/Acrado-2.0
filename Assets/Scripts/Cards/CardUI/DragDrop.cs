using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class DragDrop : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameManager gameManager;
    private GameObject canvas;
    private GameObject playerBoard;
    private PlayerView playerView;

    [SerializeField] private CardData cardData;

    private bool isDragging = false;
    private bool isDraggable = true;
    private bool isOverPlayerBoard = false;

    private GameObject startParent;
    private Vector2 startPosition;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        playerBoard = GameObject.Find("PlayerBoard");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!hasAuthority)
        {
            isDraggable = false;
            Debug.Log(isDraggable);
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
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerBoard")
        {
            isOverPlayerBoard = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "PlayerBoard")
        {
            isOverPlayerBoard = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable) return;

        if (!playerView.IsMyTurn) return;

        int playerMana = playerView.MyGameState == GameState.Player1Turn ? gameManager.player1CurrentMana : gameManager.player2CurrentMana;

        if (cardData.card.manaCost > playerMana) return;

        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;

        transform.SetParent(canvas.transform, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraggable) return;

        if (!playerView.IsMyTurn) return;

        if (isDragging == false) return;

        isDragging = false;

        if (isOverPlayerBoard && playerBoard.transform.childCount < 10)
        {
            transform.SetParent(playerBoard.transform, false);
            playerView.PlayCard(gameObject);
            isDraggable = false;
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}

