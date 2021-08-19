using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerView : NetworkBehaviour
{
    private GameManager gameManager;

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
}
