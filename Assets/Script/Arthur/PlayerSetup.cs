using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] 
    private Behaviour[] componentsToDisable;

    [SerializeField] 
    private GameObject playerUIPrefeb;
    private GameObject playerUIInstance;
    public GameObject GetPlayerUIInstance { get { return playerUIInstance; } }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            // Création UI joueur local
            playerUIInstance = Instantiate(playerUIPrefeb);

            // Config UI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            ui.SetDataPlayer(GetComponent<DataPlayer>());

            CmdSetUsername(transform.name, UserAccountManager.LoggedInUsername);
        }
    }

    [Command]
    private void CmdSetUsername(string playerID, string username)
    {
        DataPlayer player = GameManager.GetPlayer(playerID);
        if (player != null)
        {
            player.username = username;
        }
    }

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
        Destroy(playerUIInstance);

        GameManager.UnregisterPlayer(transform.name);
    }
}
