using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class setting_Set : MonoBehaviour
{
#region Элементы UI
    [SerializeField]
    private TMP_InputField NickName;

    [SerializeField]
    private TMP_InputField SensitivityCamera;
    [SerializeField]
    private Button ControllerId;
    [SerializeField]
    private Button SensitivityId;

    [SerializeField]
    private Button BackgroundId;
    public GameObject[] Backgrounds;
    
#endregion 

#region Выгрузка данных
    private void Start()
    {
        if(PlayerPrefs.HasKey("NickName"))
        {
            setting_Save.NickName = PlayerPrefs.GetString("NickName");
            NickName.placeholder.GetComponent<TMP_Text>().text = PlayerPrefs.GetString("NickName");
        }



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



        if(PlayerPrefs.HasKey("BackgroundId"))
        {
            setting_Save.BackgroundId = PlayerPrefs.GetInt("BackgroundId");
            BackgroundId.GetComponentInChildren<TMP_Text>().text = NameBackgroundId();
            for(int i = 0; i < Backgrounds.Length; i++)
            {
                if(i == setting_Save.BackgroundId) Backgrounds[i].SetActive(true);
                else Backgrounds[i].SetActive(false);
            }
        } 
    }
#endregion

#region Имя игрока
    public void SetNickName()
    {
        if(NickName.text != null) setting_Save.NickName = NickName.text;
        PlayerPrefs.SetString("NickName", NickName.text);
        NickName.placeholder.GetComponent<TMP_Text>().text = PlayerPrefs.GetString("NickName");
        NickName.text = null;
    }
#endregion

#region Чувствительность камеры
    public void SetSensitivityCamera()
    {
        if(SensitivityCamera.text != null) setting_Save.SensitivityCamera = float.Parse(SensitivityCamera.text);
        PlayerPrefs.SetFloat("SensitivityCamera", float.Parse(SensitivityCamera.text));
        SensitivityCamera.placeholder.GetComponent<TMP_Text>().text = "чувствительность: " + PlayerPrefs.GetFloat("SensitivityCamera").ToString();
        SensitivityCamera.text = null;
    }
#endregion
#region ID контроллера тела
    public void SetControllerId()
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
    public void SetSensitivityId()
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

#region ID Заднего фона
    public void SetBackgroundId()
    {
        setting_Save.BackgroundId++;
        if(setting_Save.BackgroundId > 1) setting_Save.BackgroundId = 0;
        PlayerPrefs.SetInt("BackgroundId", setting_Save.BackgroundId);

        BackgroundId.GetComponentInChildren<TMP_Text>().text = NameBackgroundId();
        for(int i = 0; i < Backgrounds.Length; i++)
        {
            if(i == setting_Save.BackgroundId) Backgrounds[i].SetActive(true);
            else Backgrounds[i].SetActive(false);
        }
    }
    private string NameBackgroundId()
    {
        if(setting_Save.BackgroundId == 0) return "Пустой фон";
        if(setting_Save.BackgroundId == 1) return "Космос";
        return "Задний фон";
    }
#endregion
}
