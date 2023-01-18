using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class menu_networkConnectItem : MonoBehaviour
{
    public NetworkManager networkManager;
    private InputField InputIP;
    private void Start()
    {
        GetComponent<InputField>();
    }

    public void Connect()
    {
        networkManager.networkAddress = InputIP.text;
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.StartClient();
        }
    }

    public void Create()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.StartHost();
        }
    }
}
