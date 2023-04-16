using System;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class MouseLook : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Mouse Settings:")]
    [Range(0f,1000f)] public float mouseSensitivity = 100f;
    [Header("Mouse Components:")]
    public Camera mainCamera;
    public Transform playerBody;
    public bool touch;
 
    private float xRotation = 0f;


    public Transform savedRotation;
 
    private void FixedUpdate()
    {
        if (touch)
        {
            Debug.Log("touch");
            MouseMovement();
        }
    }
 
    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        touch = true;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        touch = false;
    }

    
}