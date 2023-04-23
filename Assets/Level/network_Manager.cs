using Mirror;
using UnityEngine;

//inherit from NetworkManager to extend its functionality
//наследуемся от NetworkManager, чтобы дополнить его функционал
public class network_Manager : NetworkManager {
    private bool player_Spawned;
    private bool player_Connected;



    public override void OnStartServer() {
        base.OnStartServer();

        //specify which struct must come to the server for the object creation
        //указываем, какой struct должен прийти на сервер, чтобы выполнилось создание объекта
        NetworkServer.RegisterHandler<networkMassage_Player>(OnCreateCharacter);
    }

    public override void OnClientConnect() {
        base.OnClientConnect();
        player_Connected = true;
    }

    public void ActivatePlayerSpawn() {
        Vector3 pos = Vector3.zero;
        //pos.z = 10f;
        //pos = Camera.main.ScreenToWorldPoint(pos);

        //create struct of a certain type, so that the server understands what this data refers to
        //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся
        networkMassage_Player m = new networkMassage_Player() {
            position = pos
        };


        //send a message to the server with the coordinates of the object creation
        //отправляем сообщение на сервер с координатами создания объекта
        NetworkClient.Send(m);
        player_Spawned = true;
    }

    public void OnCreateCharacter(NetworkConnectionToClient conn, networkMassage_Player message) {
        //create a gameObject locally on the server
        //локально на сервере создаем gameObject
        GameObject go = Instantiate(playerPrefab, message.position, Quaternion.identity);

        //attach gameObject to the network object pool and send information about it to the other players
        //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
        NetworkServer.AddPlayerForConnection(conn, go);
    }
}

//inherit from the NetworkMessage interface, so that the system understands what data to pack
//наследуемся от интерфейса NetworkMessage, чтобы система поняла какие данные упаковывать
public struct networkMassage_Player : NetworkMessage {
    //you can't use a property
    //нельзя использовать property
    public Vector3 position;
}