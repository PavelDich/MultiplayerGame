using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class menu_networkConnect : MonoBehaviour
{
    public GameObject Player;
    public NetworkManager networkManager;
    private InputField InputIP;
    private void Start()
    {
        InputIP = GetComponent<InputField>();
    }

    public void Connect()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.networkAddress = InputIP.text;//"192.168.1.135";
            networkManager.StartClient();
            SceneManager.LoadScene("GameScane");
        }
    }

    public void Create()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkManager.StartHost();
            SceneManager.LoadScene("GameScane");
        }
    }
}
