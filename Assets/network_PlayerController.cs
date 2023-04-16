using Mirror;
using System.Collections.Generic;
using UnityEngine;

//mark the object as a network object by inheriting from NetworkBehaviour
//помечаем объект как сетевой, унаследовавшись от NetworkBehaviour
public class network_PlayerController : NetworkBehaviour 
{
    public bool localPlayer;
    public float speedBody;
    public float speedHead;
    [SerializeField] private Joystick ControllerBody;
    [SerializeField] private Transform Body;
    [SerializeField] private Transform Head;
    public TouchField touchField;
    public float yRotation;
    public float xRotation;

    void Start()
    {
        //if(!isOwned) Head.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        //check the ownershop of the object
        //проверяем, есть ли у нас права изменять этот объект
        if (isOwned || localPlayer) 
        {
            float BodyPositionX = Input.GetAxis("Horizontal") * speedBody * Time.deltaTime;
            float BodyPositionZ = Input.GetAxis("Vertical") * speedBody * Time.deltaTime;
            transform.Translate(new Vector3(BodyPositionX, 0f, BodyPositionZ));
            /*
            yRotation -= touchField.TouchDist.y * Time.deltaTime * speedHead;
            xRotation -= -touchField.TouchDist.x * Time.deltaTime * speedHead;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);
            Head.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
            */
        }
    }
}