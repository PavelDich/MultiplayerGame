using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            network_PlayerController player = col.GetComponent<network_PlayerController>();
            player.Death();
        }
        
    }
}
