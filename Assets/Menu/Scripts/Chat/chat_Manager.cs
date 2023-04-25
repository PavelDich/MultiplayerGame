using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;
using System;

public class chat_Manager : NetworkBehaviour{
    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TMP_Text AllMassage = null;
    [SerializeField] private GameObject canvas = null;

    public void HandleMessage()
    {
    }

    public void SendMessageToServer(string message)
    {
        AllMassage.text += message + "\n";
        inputField.text = string.Empty;
    }
}