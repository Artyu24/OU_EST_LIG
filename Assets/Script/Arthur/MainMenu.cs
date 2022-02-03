using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{

    private NetworkManager networkManager;


    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    /*public void LeaveGame()
    {
        if(isClientOnly)
            networkManager.StopClient();
        else
            networkManager.StopHost();
    }*/

    public void HostGame()
    {
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        networkManager.StartClient();
    }
}
