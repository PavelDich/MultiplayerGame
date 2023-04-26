using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class game_Controller : MonoBehaviour
{
    public static Dictionary<int, NetworkConnectionToServer> connections =
        new Dictionary<int, NetworkConnectionToServer>();
    public string[] NickName;
    public GameObject Camera;
    public GameObject NetworkManager;
    network_Manager network_Manager;
    //[SyncVar]
    public bool isGameStarted = false;

    public void Start()
    {
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
    }
    public void SpawnPlayer()
    {
        if(isGameStarted) return;
        Destroy(Camera);
        network_Manager.ActivatePlayerSpawn();
    }
}
