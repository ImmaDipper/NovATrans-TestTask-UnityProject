using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Загрузка основной рабчей сцены
    public void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Выход из приложения
    public void Exit()
    {
        Application.Quit();
    }
}
