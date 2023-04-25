using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class menu_Controller : MonoBehaviour
{
    public network_Manager network_Manager;
    [SerializeField] private Button startGame;
    public TMP_InputField InputIP;
    public TMP_Text IPAdress;
    private void Start()
    {
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
        
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAdress.text = "Выш ip адрес: \n" + ip.ToString();
                }
            }
    }
    public void Connect(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            network_Manager.networkAddress = InputIP.text;
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
            //network_Manager.ServerChangeScene(SceneName);
        }
    }

    public void Disconect(int SceneName)
    {
        network_Manager.StopClient();
        network_Manager.StopHost();
        SceneManager.LoadScene(SceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
