using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level_networkSpawnPlayers : NetworkManager
{
    bool playerSpawned;
    bool playerConnected;

    public void OnCreateCharacter(NetworkConnectionToClient conn, PosMessage message)
    {
        GameObject go = Instantiate(playerPrefab, message.vector3, Quaternion.identity); //локально на сервере создаем gameObject
        NetworkServer.AddPlayerForConnection(conn, go); //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PosMessage>
        (OnCreateCharacter); //указываем, какой struct должен прийти на сервер, чтобы выполнился свапн
    }

    public void ActivatePlayerSpawn()
    {
        Vector3 pos = Vector3.zero;

        PosMessage m = new PosMessage() { vector3 = pos }; //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся
        NetworkClient.Send(m); //отправка сообщения на сервер с координатами спавна
        playerSpawned = true;
    }

    public override void OnClientConnect()
    {
        playerConnected = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerSpawned && playerConnected)
        {
            ActivatePlayerSpawn();
        }
    }
}

public struct PosMessage : NetworkMessage //наследуемся от интерфейса NetworkMessage, чтобы система поняла какие данные упаковывать
{
    public Vector3 vector3; //нельзя использовать Property
}
