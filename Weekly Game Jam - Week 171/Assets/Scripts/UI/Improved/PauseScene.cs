using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    private int mainMenu = 0;
    private int pauseMenu = 1;
    private int prototype = 2;

    public void Resume()
    {
        SceneManager.LoadScene(prototype);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
