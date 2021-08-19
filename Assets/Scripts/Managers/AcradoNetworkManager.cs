using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AcradoNetworkManager : NetworkManager
{
    [SerializeField] private GameObject gameManagerPrefab;
    [HideInInspector] public GameManager GameManager;
    public override void OnStartClient()
    {
        base.OnStartClient();
        GameObject gameManagerObject = Instantiate(gameManagerPrefab);
        GameManager GameManager = gameManagerObject.GetComponent<GameManager>();
    }
}
