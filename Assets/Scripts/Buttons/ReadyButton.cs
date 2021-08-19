using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ReadyButton : NetworkBehaviour
{
    private GameManager gameManager;

    public void OnClick(bool isOn)
    {
        Debug.Log(isOn);

        if (gameManager == null)
        {
            Debug.Log(connectionToServer);
            gameManager = connectionToServer.identity.GetComponent<AcradoNetworkManager>().GameManager;
        }
        gameManager.CmdIsPlayerReady(isOn);
    }
}
