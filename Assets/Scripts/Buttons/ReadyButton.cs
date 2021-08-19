using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ReadyButton : NetworkBehaviour
{
    private GameManager GameManager;
    public void OnClick(bool isOn)
    {
        if(GameManager == null)
        {
            GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        GameManager.CmdIsPlayerReady(isOn);
    }
}
