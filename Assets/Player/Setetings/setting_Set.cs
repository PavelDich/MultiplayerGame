using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class setting_Set : MonoBehaviour
{
#region Элементы UI
    [SerializeField]
    private TMP_InputField SensitivityCamera;
    [SerializeField]
    private Button ControllerId;
    [SerializeField]
    private Button SensitivityId;
#endregion 

#region Выгрузка данных
    private void Start()
    {
        if(PlayerPrefs.HasKey("SensitivityCamera"))
        {
            setting_Save.SensitivityCamera = PlayerPrefs.GetFloat("SensitivityCamera");
            SensitivityCamera.placeholder.GetComponent<TMP_Text>().text = "чувствительность: " + PlayerPrefs.GetFloat("SensitivityCamera").ToString();
        }
        if(PlayerPrefs.HasKey("ControllerId"))
        {
            setting_Save.ControllerId = PlayerPrefs.GetInt("ControllerId");
            ControllerId.GetComponentInChildren<TMP_Text>().text = NameControllerId();
        }
        if(PlayerPrefs.HasKey("SensitivityId"))
        {
            setting_Save.SensitivityId = PlayerPrefs.GetInt("SensitivityId");
            SensitivityId.GetComponentInChildren<TMP_Text>().text = NameSensitivityId();
        } 
    }
#endregion

#region Чувствительность камеры
    private void SetSensitivityCamera()
    {
        if(SensitivityCamera.text != null) setting_Save.SensitivityCamera = float.Parse(SensitivityCamera.text);
        PlayerPrefs.SetFloat("SensitivityCamera", float.Parse(SensitivityCamera.text));
        SensitivityCamera.placeholder.GetComponent<TMP_Text>().text = "чувствительность: " + PlayerPrefs.GetFloat("SensitivityCamera").ToString();
        SensitivityCamera.text = null;
    }
#endregion
#region ID контроллера тела
    private void SetControllerId()
    {
        setting_Save.ControllerId++;
        if(setting_Save.ControllerId > 1) setting_Save.ControllerId = 0;
        PlayerPrefs.SetInt("ControllerId", setting_Save.ControllerId);

        ControllerId.GetComponentInChildren<TMP_Text>().text = NameControllerId();
    }
    private string NameControllerId()
    {
        if(setting_Save.ControllerId == 0) return "Клавиатура";
        if(setting_Save.ControllerId == 1) return "Статический джостик";
        return "Тип управления передвижением";
    }
#endregion
#region ID Контроллера камеры
    private void SetSensitivityId()
    {
        setting_Save.SensitivityId++;
        if(setting_Save.SensitivityId > 1) setting_Save.SensitivityId = 0;
        PlayerPrefs.SetInt("SensitivityId", setting_Save.SensitivityId);

        SensitivityId.GetComponentInChildren<TMP_Text>().text = NameSensitivityId();
        
    }
    private string NameSensitivityId()
    {
        if(setting_Save.SensitivityId == 0) return "Мышка";
        if(setting_Save.SensitivityId == 1) return "Сенсорная панель";
        return "Тип управления камерой";
    }
#endregion
}
