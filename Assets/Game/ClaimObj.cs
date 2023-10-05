using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimObj : MonoBehaviour
{
    private player_Host player_Host;
    private player_Controller player_Controller;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player"){
            player_Host = col.GetComponent<player_Host>();
            player_Controller = col.GetComponent<player_Controller>();
        player_Host.Diploms = player_Host.Diploms - 1;
        if(player_Host.Diploms <= 0)player_Controller.Disconect(0); 
        Destroy(gameObject);
        }
       
    }
}
