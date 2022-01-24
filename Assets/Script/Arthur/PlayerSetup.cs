using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        RegisterPlayerAndSetUsername();
    }

    /*
    // Utilisé dans le cas où le build est uniquement un serveur
    public override void OnStartServer()
    {
        base.OnStartServer();

        RegisterPlayerAndSetUsername();
    }
    */

    private void RegisterPlayerAndSetUsername()
    {
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        DataPlayer player = GetComponent<DataPlayer>();

        GameManager.RegisterPlayer(netId, player);
    }

    private void OnDisable()
    {
        GameManager.UnregisterPlayer(transform.name);
    }
}
