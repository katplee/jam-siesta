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
        SceneManager.LoadScene(mainMenu);
    }

    
}
