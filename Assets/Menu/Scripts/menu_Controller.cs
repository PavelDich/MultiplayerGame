using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

public class menu_Controller : MonoBehaviour
{
    public network_Manager network_Manager;
    [SerializeField] private GameObject loading;
    public TMP_InputField InputIP;
    private void Start()
    {
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
    }
    
    public void Create(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            loading.SetActive(true);
            network_Manager.StartHost();
            SceneManager.LoadScene(SceneName);
        }
    }
    public void Connect_PC(int SceneName)
    {
        if(!Input.GetKeyDown(KeyCode.Return)) { return; }
        Connect(SceneName);
    }
    public void Connect(int SceneName)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active && !string.IsNullOrWhiteSpace(InputIP.text))
        {
            loading.SetActive(true);
            network_Manager.networkAddress = InputIP.text;
            network_Manager.StartClient();
            if(network_Manager.isNetworkActive) SceneManager.LoadScene(SceneName);
            InputIP.text = string.Empty;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}



