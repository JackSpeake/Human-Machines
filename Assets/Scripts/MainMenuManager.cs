using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button quitButton;

    private void Start()
    {
        // CHECK IF WEB BUILD
        // IF WEB BUILD REMOVE QUIT BUTTON
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}
