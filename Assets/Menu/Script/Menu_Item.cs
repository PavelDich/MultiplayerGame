using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Item : MonoBehaviour
{
    public string String_Name;
    

    
    public void Void_Open()
    {
        gameObject.SetActive(true);
    }
    public void Void_Close()
    {
        gameObject.SetActive(false);
    }
}
