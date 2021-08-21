using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerView : NetworkBehaviour
{
    public static HashSet<PlayerView> ActivePlayers = new HashSet<PlayerView>();
    [SyncVar] public int MyID = -1;

    private GameState myGameState;
    private bool isMyTurn;

    private GameManager gameManager;
    private CardManager cardManager;

    protected virtual void Start()
    {
        if (isServer)
        {
            ActivePlayers.Add(this);
            MyID = ActivePlayers.Count;
            Debug.Log(MyID);
        }
    }

    protected virtual void OnDestroy()
    {
        ActivePlayers.Remove(this);
    }

    public void ReadySate(bool isReady)
    {
        GameManager.CmdIsPlayerReady(isReady);
    }

    public void DrawCard(int amount)
    {
        CardManager.CmdDrawCards(amount);
    }

    public void PlayCard(GameObject cardObject)
    {
        if (IsMyTurn)
        {
            //do stuff
        }
    }

    public bool IsMyTurn
    {
        get
        {
            if (MyGameState == gameManager.gameState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            isMyTurn = value;
        }
    }

    public GameState MyGameState
    {
        get
        {
            if(MyID == GameManager.player1ID)
            {
                return GameState.Player1Turn;
            }
            else if (MyID == GameManager.player2ID)
            {
                return GameState.Player2Turn;
            }
            else
            {
                Debug.Log("Shit Connection LMAO");
                return GameState.Setup;
            }
        }
    }

    public GameManager GameManager
    {
        get
        {
            if(gameManager == null)
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            return gameManager;
        }
    }

    public CardManager CardManager
    {
        get
        {
            if (cardManager == null)
            {
                cardManager = GameObject.Find("GameManager").GetComponent<CardManager>();
            }

            return cardManager;
        }
    }
}
