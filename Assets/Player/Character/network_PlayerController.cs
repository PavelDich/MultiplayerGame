using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class network_PlayerController : NetworkBehaviour 
{
    [SerializeField] private float speedBody;
    [SerializeField] private Joystick ControllerBody;
    [SerializeField] private Transform Head;
    [SerializeField] private Transform Camera;
    [SerializeField] private Animator AnimatiorMenu;
    public bool itsMe;

    private void Start()
    {
        itsMe = isOwned;
        if(!isOwned) Camera.gameObject.SetActive(false);
        if(setting_Save.ControllerId == 0) ControllerBody.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        #region Управление телом
        if (isOwned) 
        {
            //Управление для ПК (WASD)
            if(setting_Save.ControllerId == 0)
            {
                float BodyPositionX = Input.GetAxis("Horizontal") * speedBody * Time.deltaTime;
                float BodyPositionZ = Input.GetAxis("Vertical") * speedBody * Time.deltaTime;
                transform.Translate(new Vector3(BodyPositionX, 0f, BodyPositionZ));
            }
            //Управление для телефона (Джостик)
            else if(setting_Save.ControllerId == 1)
            {
                float BodyPositionX = ControllerBody.Horizontal * speedBody * Time.deltaTime;
                float BodyPositionZ = ControllerBody.Vertical * speedBody * Time.deltaTime;
                transform.Translate(new Vector3(BodyPositionX, 0f, BodyPositionZ));
            }
        }
        #endregion
        
        if(Input.GetKey(KeyCode.Tab))
        {
            //AnimatiorMenu
        }
    }

    public void Death()
    {
        Debug.Log("deach");
    }
}