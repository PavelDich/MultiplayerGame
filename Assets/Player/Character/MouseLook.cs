using System;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class MouseLook : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Mouse Settings:")]
    [Range(0f,1000f)] private float mouseSensitivity = setting_Save.SensitivityCamera;
    [Header("Mouse Components:")]
    public Transform playerBody;
    public Transform playerHead;
    private bool Pressed;
    private Vector2 TouchPosition;
    private Vector2 TouchPositionOld;
    private int TouchId;
 
    private float xRotation = 0f;
    private float yRotation = 0f;



    private void Start()
    {
        if(setting_Save.SensitivityId == 0) Cursor.visible = false; 
    }
    private void Update()
    {
        //Управление камерой
        #region 
        //Управление с ПК (Мышка)
        if(setting_Save.SensitivityId == 0)
        {
            xRotation += Input.GetAxis("Mouse X") * mouseSensitivity * 0.02f;
            yRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * 0.02f;
            playerHead.localRotation = Quaternion.Euler(Mathf.Clamp(yRotation, -80f, 80f), 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, xRotation, 0f);
        }
        //Управление с телефона (Сенсорная панель)
        if(setting_Save.SensitivityId == 1)
        {
            if (Pressed)
            {
                TouchPosition = Input.touches[TouchId].deltaPosition;
            }
            else
            {
                TouchPosition = new Vector2();
            }
        
            xRotation += TouchPosition.x * mouseSensitivity * 0.02f;
            yRotation -= TouchPosition.y * mouseSensitivity * 0.02f;
            playerHead.localRotation = Quaternion.Euler(Mathf.Clamp(yRotation, -80f, 80f), 0f, 0f);
            playerBody.rotation = Quaternion.Euler(0f, xRotation, 0f);
        }
        #endregion
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        TouchId = eventData.pointerId;
        TouchPositionOld = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}