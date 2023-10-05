using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Attack : MonoBehaviour
{
    public player_Controller player_Controller;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && player_Controller.IsSecurity == true)
        {
            player_Controller player = col.GetComponent<player_Controller>();
            player.ChangeIsSecurity(true);
        }
    }
}