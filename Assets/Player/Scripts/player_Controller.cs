using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Mirror;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class player_Controller : NetworkBehaviour 
{
    public network_Manager network_Manager;
    public game_Controller game_Controller;
    public GameObject[] Players;
    public player_Host player_Host;
    public TMP_Text IPAdress;

    [SerializeField] private TMP_Text playerNickName;
    [SyncVar(hook = nameof(SyncNickName))] 
    public string _NickName;

    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;
    private static event Action<string> OnMessage;

    [SerializeField] private GameObject Controllers;
    private bool ControllerActive = true;
    [SerializeField] private float speedBody;
    [SerializeField] private Joystick ControllerBody;

    [SerializeField] private GameObject Camera;
    [SerializeField] private MouseLook MouseLook;

    [SerializeField] private GameObject MobilePanel;

    [SerializeField] private Animator AnimatiorMenu;
    [SerializeField] private setting_Set setting_Set;
    
    

    private void Start()
    {
            network_Manager = GameObject.Find("NetworkManager").GetComponent<network_Manager>();
            game_Controller = GameObject.Find("gameController").GetComponent<game_Controller>();
            Players = GameObject.FindGameObjectsWithTag("Player");
        if(!isOwned){ 
            Destroy(Camera);
        }else{
            if(setting_Save.ControllerId == 0) MobilePanel.gameObject.SetActive(false);
        }

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAdress.text = "Выш ip адрес: \n" + ip.ToString();
                }
            }

        if(isServer){
            ChangeNickName(setting_Save.NickName);
            if(isOwned) player_Host.ChangeIsHost(true);
        }else{
            CmdChangeNickName(setting_Save.NickName);
            for(int i = 0; i < Players.Length; i++)
            {
                if(Players[i].GetComponent<player_Host>().isHost == true)
                player_Host = Players[i].GetComponent<player_Host>();
            }
        } 
    }
    public override void OnStartAuthority()
    {
        OnMessage += HandleNewMessage;
    }
    private void FixedUpdate()
    {
        
        if (isOwned) 
        {
        #region Управление телом
            //Управление для ПК (WASD)
            if(setting_Save.ControllerId == 0 && ControllerActive)
            {
                float BodyPositionX = Input.GetAxis("Horizontal") * speedBody * Time.deltaTime;
                float BodyPositionZ = Input.GetAxis("Vertical") * speedBody * Time.deltaTime;
                transform.Translate(BodyPositionX, 0f, BodyPositionZ);
            }
            //Управление для телефона (Джостик)
            else if(setting_Save.ControllerId == 1 && ControllerActive)
            {
                float BodyPositionX = ControllerBody.Horizontal * speedBody * Time.deltaTime;
                float BodyPositionZ = ControllerBody.Vertical * speedBody * Time.deltaTime;
                transform.Translate(BodyPositionX, 0f, BodyPositionZ);
            }
        #endregion



            if(Input.GetKeyDown(KeyCode.Tab))
            {
                OpenGameMenu();
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                if(isServer){
                    player_Host.ChangeGameStarted(player_Host.GameStarted = !player_Host.GameStarted);
                }
            }
        }
    }

    public void OpenGameMenu()
    {
        setting_Set.Start();
        ControllerActive = !ControllerActive;
        if(!ControllerActive) {
            Cursor.visible = true;
            MouseLook.enabled = false;
            AnimatiorMenu.SetInteger("menu_ID", 1); 
            AnimatiorMenu.SetInteger("menu_Layer", 1);
        }else{
            Cursor.visible = false;
            MouseLook.enabled = true;
            AnimatiorMenu.SetInteger("menu_ID", 1); 
            AnimatiorMenu.SetInteger("menu_Layer", 0);
        }
        
        if(setting_Save.ControllerId == 0) MobilePanel.gameObject.SetActive(false);
        else MobilePanel.gameObject.SetActive(true);

        if(setting_Save.ControllerId == 1) Controllers.SetActive(ControllerActive);
    }



#region Имя игрока
    void SyncNickName(string oldValue, string newValue)
    {
        playerNickName.text = newValue;
    }

    [Server]
    public void ChangeNickName(string newValue)
    {
        _NickName = newValue;
    }

    [Command]
    public void CmdChangeNickName(string newValue)
    {
        ChangeNickName(newValue);
    }
#endregion

#region Чат
    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

    public void Send_PC()
    {
        if(!Input.GetKeyDown(KeyCode.Return)) { return; }
        Send();
    }
    [Client]
    public void Send()
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{_NickName}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
#endregion

    [ClientRpc]
    public void Death()
    {
        Debug.Log("deach");
    }

    public void Disconect(int SceneName)
    {
        network_Manager.StopClient();
        network_Manager.StopHost();
        SceneManager.LoadScene(SceneName);
    }
}