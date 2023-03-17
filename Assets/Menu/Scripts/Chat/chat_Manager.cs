using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;
using TMPro;

public class chat_Manager : NetworkBehaviour
{
    public Transform chat_Content;
    public chat_Item сhat_Item;
    public TMP_InputField chat_Input;
    
    private static event Action<string> OnMessage;

    
    public void Send()
    {
        if (isServer)
            send_CmdMessage(chat_Input.text);
        else
            send_Message(chat_Input.text);
    }

    [Server]
    public void send_Message(string massage)
    {
        сhat_Item.Massage = massage;
        GameObject massagePref = Instantiate(сhat_Item.gameObject, chat_Content); //Создаем локальный объект пули на сервере
        
        NetworkServer.Spawn(massagePref); //отправляем информацию о сетевом объекте всем игрокам.
    }


    [Command]
    public void send_CmdMessage(string massage)
    {
        send_Message(massage);
    }
}
