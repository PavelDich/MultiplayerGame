using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class menu_networkConnect : MonoBehaviour
{
    public NetworkManager networkManager;
    private InputField InputIP;
    public TMP_Text IPAdress;
    private void Start()
    {
        InputIP = GetComponent<InputField>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if(InputIP != null)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
             foreach (var ip in host.AddressList)
             {
                 if (ip.AddressFamily == AddressFamily.InterNetwork)
                 {
                    IPAdress.text = ip.ToString();
                 }
             }
        }
        
    }

    public void Connect(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.networkAddress = InputIP.text;//"192.168.1.135";
            networkManager.StartClient();
            SceneManager.LoadScene(SceneName);
        }
    }

    public void Create(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.StartHost();
            SceneManager.LoadScene(SceneName);
        }
    }

    public void lobby_StartGame(int SceneName)
    {
        SceneManager.LoadScene(SceneName);
        //networkManager.playerPrefab = Instantiate(Player);
    }
}
