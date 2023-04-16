using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class chat_Manager : NetworkBehaviour
{
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private GameObject MassagePrefab;
    [SerializeField] private Transform Content;

    public void SetMassageText()
    {
        if(!Input.GetKeyDown(KeyCode.Return)) { return; }
        SpawnMassage(inputField.text);
    }

    [ClientRpc]
    public void SpawnMassage(string massage)
    {
        MassagePrefab.GetComponent<chat_Item>().Massage = massage;
        network_Manager.Instantiate(MassagePrefab, Content.position, Quaternion.identity);
    }
}