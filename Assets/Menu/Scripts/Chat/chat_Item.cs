using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class chat_Item : MonoBehaviour
{
    public string Massage;
    [SerializeField]private TMP_Text UiMassage;
    void Start()
    {
        UiMassage.text = Massage;
    }
}
