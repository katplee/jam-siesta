using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    private int mainMenu = 0;
    private int prototype = 1;

    public void Play()
    {
        SceneManager.LoadScene(prototype);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
