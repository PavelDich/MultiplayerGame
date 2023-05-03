using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Mirror;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class player_Host : NetworkBehaviour
{
#region Начало игры
    [SyncVar(hook = nameof(SyncGameStarted))]
    public bool _GameStarted;
    public bool GameStarted;

    private void SyncGameStarted(bool oldValue, bool newValue)
    {
        GameStarted = newValue;
    }

    [Server]
    public void ChangeGameStarted(bool newValue)
    {
        _GameStarted = newValue;
    }
#endregion

#region Сервер
    [SyncVar(hook = nameof(SyncIsHost))]
    public bool _isHost;
    public bool isHost;

    private void SyncIsHost(bool oldValue, bool newValue)
    {
        isHost = newValue;
    }

    [Server]
    public void ChangeIsHost(bool newValue)
    {
        _isHost = newValue;
    }
#endregion
}
