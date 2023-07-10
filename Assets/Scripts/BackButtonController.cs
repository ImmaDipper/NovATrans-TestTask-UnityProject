using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonController : MonoBehaviour
{
    // Загрузка сцены меню
    public void GoBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
