using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetedTarget : Target
{
    protected override void SetTarget()
    {
        //targetCard = NetworkClient.connection.identity.GetComponent<PlayerView>().TargetedGameobject;
    }
}
