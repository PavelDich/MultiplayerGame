using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class player_Controller : NetworkBehaviour 
{
    public bool itsMe;

    [SerializeField] private TMP_Text playerNickName;
    [SyncVar(hook = nameof(SyncNickName))] 
    public string _NickName;

    private bool ControllerActive = true;
    [SerializeField] private float speedBody;
    [SerializeField] private Joystick ControllerBody;
    [SerializeField] private Transform Head;

    [SerializeField] private Transform Camera;
    private MouseLook MouseLook;

    [SerializeField] private Animator AnimatiorMenu;
    private setting_Set setting_Set;
    
    

    private void Start()
    {
        itsMe = isOwned;
        if(!isOwned) Camera.gameObject.SetActive(false);
        else{
            MouseLook = GetComponentInChildren<MouseLook>();
            setting_Set = GetComponentInChildren<setting_Set>();
            if(setting_Save.ControllerId == 0) ControllerBody.gameObject.SetActive(false);
        }

        if(isServer){
            ChangeNickName(setting_Save.NickName);
        }else{
            ChangeNickName(setting_Save.NickName);
        }
    }
    private void FixedUpdate()
    {
        #region Управление телом
        if (isOwned) 
        {
            
            //Управление для ПК (WASD)
            if(setting_Save.ControllerId == 0 && ControllerActive)
            {
                float BodyPositionX = Input.GetAxis("Horizontal") * speedBody * Time.deltaTime;
                float BodyPositionZ = Input.GetAxis("Vertical") * speedBody * Time.deltaTime;
                transform.Translate(new Vector3(BodyPositionX, 0f, BodyPositionZ));
            }
            //Управление для телефона (Джостик)
            else if(setting_Save.ControllerId == 1 && ControllerActive)
            {
                float BodyPositionX = ControllerBody.Horizontal * speedBody * Time.deltaTime;
                float BodyPositionZ = ControllerBody.Vertical * speedBody * Time.deltaTime;
                transform.Translate(new Vector3(BodyPositionX, 0f, BodyPositionZ));
            }
        #endregion



            if(Input.GetKey(KeyCode.Tab))
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
                
            }
        }
        
    }

    public void Death()
    {
        if(isServer){
            ChangeNickName(setting_Save.NickName);
        }else{
            CmdChangeNickName(setting_Save.NickName);
        }
        Debug.Log("deach");
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


    void SyncNickName(string oldValue, string newValue)
    {
        playerNickName.text = newValue;
    }
}