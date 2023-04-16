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
    public network_Manager network_Manager;
    public TMP_InputField InputIP;
    public TMP_Text IPAdress;
    private void Start()
    {
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
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
            network_Manager.networkAddress = InputIP.text;//"192.168.1.135";
            network_Manager.StartClient();
            SceneManager.LoadScene(SceneName);
        }
    }

    public void Create(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            network_Manager.StartHost();
            SceneManager.LoadScene(SceneName);
        }
    }

    public void lobby_StartGame(int SceneName)
    {
        SceneManager.LoadScene(SceneName);
        //networkManager.playerPrefab = Instantiate(Player);
    }

    public void exit(int SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
