using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Joystick joystickBody;
    [SerializeField] private Transform playerCamera;
    private float xRotation;
    private float yRotation;
    public float speedB;
    public float speedC;

    public TouchField touchField;

    void Start()
    {
        if(!isLocalPlayer) playerCamera.gameObject.SetActive(false);
    }
    void Update()
    {
        transform.Translate (new Vector3(joystickBody.Horizontal * Time.deltaTime * speedB, 0f , joystickBody.Vertical * Time.deltaTime * speedB));
        transform.Translate (new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speedB, 0f , Input.GetAxis("Vertical") * Time.deltaTime * speedB));

            float Xrot =  touchField.TouchAxis.x * Time.deltaTime * speedC;
            float Yrot =  touchField.TouchAxis.y * Time.deltaTime * speedC;

            yRotation -= Yrot;
            xRotation -= -Xrot;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);



    }
}

