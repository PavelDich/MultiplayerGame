using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using Mirror;
using System.Net;
using System.Net.Sockets;
using TMPro;

public class player_Controller : NetworkBehaviour 
{
    public network_Manager network_Manager;
    public game_Controller game_Controller;
    public Rigidbody _Rigidbody;
    public GameObject[] Players;
    public GameObject Survivel;
    public GameObject Security;
    public player_Host player_Host;
    public TMP_Text IPAdress;

    [SyncVar(hook = nameof(SyncIsSecurity))] 
    public bool _IsSecurity;
    public bool IsSecurity = false;

    public bool IsMe = false;

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
    [SerializeField] private Animator AnimatiorWalk;
    private float BodyPositionX;
    private float BodyPositionZ;

    [SerializeField] private GameObject Camera;
    [SerializeField] private MouseLook MouseLook;

    [SerializeField] private GameObject MobilePanel;

    [SerializeField] private Animator AnimatiorMenu;
    [SerializeField] private setting_Set setting_Set;
    
    

    private void Start()
    {
        IsMe = isOwned;
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

    public void RestartGame()
    {
        Security.SetActive(false);
        Survivel.SetActive(true);
        player_Host.ChangeGameStarted(false);
    }
    public void StartGame()
    {
        if(player_Host.GameStarted == false){
        player_Host.GameStarted = true;
        player_Host.ChangeGameStarted(true);
        if(isServer) Players[Random.Range(0, Players.Length)].GetComponent<player_Controller>().ChangeIsSecurity(true);
        }
    }
    private void FixedUpdate()
    {/*
        if(IsSecurity = true && player_Host.GameStarted == true) {
        Security.SetActive(true);
        Survivel.SetActive(false);
        }

        if(IsSecurity = false && player_Host.GameStarted == true) {
        Security.SetActive(false);
        Survivel.SetActive(true);
        }
        */

        
        if (isOwned) 
        {
            
        #region Управление телом
            if(setting_Save.ControllerId == 0 && ControllerActive)
            {
                //Управление для ПК (WASD)

                //BodyPositionX = Input.GetAxis("Horizontal") * speedBody * Time.deltaTime;
                //BodyPositionZ = Input.GetAxis("Vertical") * speedBody * Time.deltaTime;
                //transform.Translate(BodyPositionX, 0f, BodyPositionZ);

                Vector3 moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
                moveDirection.y = 0f;
                _Rigidbody.velocity = moveDirection * speedBody + new Vector3(0f, _Rigidbody.velocity.y, 0f);
            }
            else if(setting_Save.ControllerId == 1 && ControllerActive)
            {
                //Управление для телефона (Джостик)

                //BodyPositionX = ControllerBody.Horizontal * speedBody * Time.deltaTime;
                //BodyPositionZ = ControllerBody.Vertical * speedBody * Time.deltaTime;
                //transform.Translate(BodyPositionX, 0f, BodyPositionZ);

                Vector3 moveDirection = transform.TransformDirection(new Vector3(ControllerBody.Horizontal, 0f, ControllerBody.Vertical));
                moveDirection.y = 0f;
                _Rigidbody.velocity = moveDirection * speedBody + new Vector3(0f, _Rigidbody.velocity.y, 0f);
            }
        #endregion



            if(Input.GetKeyDown(KeyCode.Tab))
            {
                OpenGameMenu();
            }
        }

        
        if(BodyPositionZ > 0f || BodyPositionX > 0f || BodyPositionZ < 0f || BodyPositionX < 0f){
                    AnimatiorWalk.SetBool("IsRanning", true);
        }else{
                    AnimatiorWalk.SetBool("IsRanning", false);
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

#region Охранник
    void SyncIsSecurity(bool oldValue, bool newValue)
    {
        IsSecurity = newValue;
        Security.SetActive(IsSecurity);
        Survivel.SetActive(!IsSecurity);
    }

    [Server]
    public void ChangeIsSecurity(bool newValue)
    {
        _IsSecurity = newValue;
    }

    [Command]
    public void CmdIsSecurity(bool newValue)
    {
        ChangeIsSecurity(newValue);
    }
#endregion

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