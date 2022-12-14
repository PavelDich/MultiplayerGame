using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class Player : NetworkBehaviour
{
    bool facingRight = true;
    public static Player localPlayer;
    public TextMesh NameDisplayText;
    [SyncVar(hook = "DisplayPlayerName")] public string PlayerDisplayName;
    [SyncVar] public string matchID;

    [SyncVar] public Match CurrentMatch;
    public GameObject PlayerLobbyUI;
    private Guid netIDGuid;

    private NetworkMatch networkMatch;

    private void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            CmdSendName(MainMenu.instance.DisplayName);
        }
    }

    public override void OnStartServer()
    {
        netIDGuid = netId.ToString().ToGuid();
        networkMatch.matchId = netIDGuid;
    }

    public override void OnStartClient()
    {
        if(isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            PlayerLobbyUI = MainMenu.instance.SpawnPlayerUIPrefab(this);
        }
    }

    public override void OnStopClient()
    {
        ClientDisconnect();
    }

    public override void OnStopServer()
    {
        ServerDisconnect();
    }

    [Command]
    public void CmdSendName(string name)
    {
        PlayerDisplayName = name;
    }

    public void DisplayPlayerName(string name, string playerName)
    {
        name = PlayerDisplayName;
        Debug.Log("Имя " + name + " : " + playerName);
        NameDisplayText.text = playerName;
    }

    public void HostGame(bool publicMatch)
    {
        string ID = MainMenu.GetRandomID();
        CmdHostGame(ID, publicMatch);
    }

    [Command]
    public void CmdHostGame(string ID, bool publicMatch)
    {
        matchID = ID;
        if (MainMenu.instance.HostGame(ID, gameObject, publicMatch))
        {
            Debug.Log("Лобби было создано успешно");
            networkMatch.matchId = ID.ToGuid();
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("Ошибка в создании лобби");
            TargetHostGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenu.instance.HostSuccess(success, ID);
    }

    public void JoinGame(string inputID)
    {
        CmdJoinGame(inputID);
    }

    [Command]
    public void CmdJoinGame(string ID)
    {
        matchID = ID;
        if (MainMenu.instance.JoinGame(ID, gameObject))
        {
            Debug.Log("Успешное подключение к лобби");
            networkMatch.matchId = ID.ToGuid();
            TargetJoinGame(true, ID);
        }
        else
        {
            Debug.Log("Не удалось подключиться");
            TargetJoinGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenu.instance.JoinSuccess(success, ID);
    }

    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    [Command]
    void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    void ServerDisconnect()
    {
        MainMenu.instance.PlayerDisconnected(gameObject, matchID);
        RpcDisconnectGame();
        networkMatch.matchId = netIDGuid;
    }

    [ClientRpc]
    void RpcDisconnectGame()
    {
        ClientDisconnect();
    }

    void ClientDisconnect()
    {
        if(PlayerLobbyUI != null)
        {
            if(!isServer)
            {
                Destroy(PlayerLobbyUI);
            }
            else
            {
                PlayerLobbyUI.SetActive(false);
            }
        }
    }

    public void SearchGame()
    {
        CmdSearchGame();
    }

    [Command]
    void CmdSearchGame()
    {
        if(MainMenu.instance.SearchGame(gameObject, out matchID))
        {
            Debug.Log("Игра найдена успешно");
            networkMatch.matchId = matchID.ToGuid();
            TargetSearchGame(true, matchID);

            if(isServer && PlayerLobbyUI != null)
            {
                PlayerLobbyUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Поиск игры не удался");
            TargetSearchGame(false, matchID);
        }
    }

    [TargetRpc]
    void TargetSearchGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log("ID: " + matchID + "==" + ID + " | " + success);
        MainMenu.instance.SearchGameSuccess(success, ID);
    }

    [Server]
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    [TargetRpc]
    void TargetPlayerCountUpdated(int playerCount)
    {
        if(playerCount > 1)
        {
            MainMenu.instance.SetBeginButtonActive(true);
        }
        else
        {
            MainMenu.instance.SetBeginButtonActive(false);
        }
    }

    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    public void CmdBeginGame()
    {
        MainMenu.instance.BeginGame(matchID);
        Debug.Log("Игра начилась");
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {matchID} | начало");
        DontDestroyOnLoad(gameObject);
        MainMenu.instance.inGame = true;
        transform.localScale = new Vector3(0.41664f, 0.41664f, 0.41664f); //Размер вашего игрока (x, y, z)
        facingRight = true;
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (isOwned)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float speed = 6f * Time.deltaTime;
            transform.Translate(new Vector2(input.x * speed, input.y * speed));
            Animator animator = GetComponent<Animator>();
            if (input.x == 0 && input.y == 0)
            {
                animator.SetBool("walk", false);
            }
            else
            {
                animator.SetBool("walk", true);
            }
            if (!facingRight && input.x > 0)
            {
                Flip();
            }
            else if (facingRight && input.x < 0)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        if (isOwned)
        {
            facingRight = !facingRight;
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;

            Vector3 TextScale = NameDisplayText.transform.localScale;
            TextScale.x *= -1;
            NameDisplayText.transform.localScale = TextScale;
        }
    }
}