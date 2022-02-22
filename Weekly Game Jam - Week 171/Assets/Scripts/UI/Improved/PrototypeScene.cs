using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeScene : MonoBehaviour
{
    private int mainMenu = 0;
    private int prototype = 1;

    public void GoToMainMenu()
    {
        Time.timeScale = 0f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
