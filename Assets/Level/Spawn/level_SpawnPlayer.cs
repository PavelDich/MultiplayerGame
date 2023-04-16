using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_SpawnPlayer : MonoBehaviour
{
    public GameObject Camera;
    public GameObject NetworkManager;
    network_Manager network_Manager;

    public void Start()
    {
        //Destroy(Camera);
        network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
    }
    public void SpawnPlayer()
    {
        Destroy(Camera);
        network_Manager.ActivatePlayerSpawn();
    }
}
