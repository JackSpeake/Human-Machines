using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button quitButton;
    [SerializeField] private float deleteTime = .75f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private GameObject hiredText, tutorialMessage;

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
        StartCoroutine(DeleteMenuElements());
    }

    IEnumerator DeleteMenuElements()
    {
        GameObject[] mainMenuElements = GameObject.FindGameObjectsWithTag("MainMenuElement");

        foreach (GameObject m in mainMenuElements)
        {
            yield return new WaitForSeconds(deleteTime);
            Destroy(m);
        }

        yield return new WaitForSeconds(waitTime);

        hiredText.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        hiredText.SetActive(false);

        yield return new WaitForSeconds(waitTime);

        tutorialMessage.SetActive(true);

    }

    public void StartWithTutorial()
    {
        LoadGameScene();
    }

    public void StartWithoutTutorial()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }
}
