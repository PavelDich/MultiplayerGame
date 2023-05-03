using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class game_Controller : MonoBehaviour
{
    network_Manager network_Manager;
    public GameObject Camera;
    public GameObject NetworkManager;



    public void Start()
    {
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
    }
    public void SpawnPlayer()
    {
        Destroy(Camera);
        network_Manager.ActivatePlayerSpawn();
    }
}
