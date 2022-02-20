using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScene : MonoBehaviour
{
    private int mainMenu = 0;
    private int prototype = 1;

    private GameObject panel;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
    }

    public void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        panel.SetActive(false);
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
