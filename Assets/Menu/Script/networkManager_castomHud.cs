using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[DisallowMultipleComponent]
[AddComponentMenu("Network/Network Manager HUD")]
[RequireComponent(typeof(NetworkManager))]
[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
public class networkManager_castomHud : MonoBehaviour
{
#region 
    NetworkManager manager;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }
#endregion

    public InputField networkManager_idHostConnect;

    public void host_connect()
    {
        manager.networkAddress = networkManager_idHostConnect.text;
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            manager.StartClient();
        }
    }

    public void host_create()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            manager.StartHost();
        }
        
    }
}
