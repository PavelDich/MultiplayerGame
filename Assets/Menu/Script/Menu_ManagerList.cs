using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_ManagerList : MonoBehaviour
{
    public static Menu_ManagerList Code_MenuManagerList;
    [SerializeField] List<Menu_Item> List_MenuItem;
    


    public void Start()
    {
        Code_MenuManagerList = this;
    }
    public void Void_OpenMenu(string String_MenuItem_Name)
    {
        foreach(Menu_Item menu in List_MenuItem)
        {
            if(menu.String_Name == String_MenuItem_Name)
            {
                menu.Void_Open();
            }
            else
            {
                menu.Void_Close();
            }
        }
    }
}
