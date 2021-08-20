using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ReadyButton : NetworkBehaviour
{
    private PlayerView player;
    public void OnClick(bool isOn)
    {
        if(player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<PlayerView>();
        }

        player.ReadySate(isOn);
    }

}
