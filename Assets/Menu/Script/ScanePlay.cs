using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScanePlay : MonoBehaviour
{
    public void LastScane(int SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
