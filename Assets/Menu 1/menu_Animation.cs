using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_Animation : MonoBehaviour
{
    private Animator Anim;
    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void menu_ID(int menu_ID)
    {
        Anim.SetInteger("menu_ID", menu_ID);
    }
    public void menu_Layer(int menu_Layer)
    {
        Anim.SetInteger("menu_Layer", menu_Layer);
    }
}
