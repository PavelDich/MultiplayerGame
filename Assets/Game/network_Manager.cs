using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class network_Manager : NetworkManager {
    private bool player_Spawned;
    private bool player_Connected;


    public override void OnStartServer() 
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<networkMassage_Player>(OnCreateCharacter);
    }

    public override void OnClientConnect() 
    {
        base.OnClientConnect();
        player_Connected = true;
    }

    public void ActivatePlayerSpawn() 
    {
        Vector3 pos = Vector3.zero;
        networkMassage_Player m = new networkMassage_Player() {position = pos};

        NetworkClient.Send(m);
        player_Spawned = true;
    }

    public void OnCreateCharacter(NetworkConnectionToClient conn, networkMassage_Player message) 
    {
        GameObject go = Instantiate(playerPrefab, message.position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, go);
    }
}

public struct networkMassage_Player : NetworkMessage 
{
    public Vector3 position;
}