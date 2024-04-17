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
    [SerializeField] private GameObject hiredText, assignmentText1, assignmentText2, tutorialMessage;

    [SerializeField] private AudioSource gunshot, notification, click, startup, shutdown;

    bool clicked = false;

    private void Start()
    {
        // CHECK IF WEB BUILD
        // IF WEB BUILD REMOVE QUIT BUTTON

        Cursor.visible = false;
    }

    public void Quit()
    {
        click.Play();
        shutdown.Play();
        Application.Quit();
    }

    public void StartGame()
    {
        if (!clicked)
        {
            clicked = true;
            click.Play();
            startup.Play();
            StartCoroutine(DeleteMenuElements());
        }
            
    }

    IEnumerator DeleteMenuElements()
    {
        GameObject[] mainMenuElements = GameObject.FindGameObjectsWithTag("MainMenuElement");

        foreach (GameObject m in mainMenuElements)
        {
            yield return new WaitForSeconds(deleteTime);
            gunshot.Play();
            Destroy(m);
        }

        yield return new WaitForSeconds(waitTime);

        hiredText.SetActive(true);
        gunshot.Play();

        yield return new WaitForSeconds(waitTime);

        hiredText.SetActive(false);

        yield return new WaitForSeconds(waitTime);

        assignmentText1.SetActive(true);
        gunshot.Play();

        yield return new WaitForSeconds(waitTime);

        assignmentText2.SetActive(true);
        gunshot.Play();

        yield return new WaitForSeconds(waitTime * 1.5f);

        assignmentText1.SetActive(false);
        assignmentText2.SetActive(false);

        yield return new WaitForSeconds(waitTime);

        tutorialMessage.SetActive(true);
        notification.Play();

    }

    public void StartWithTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        LoadGameScene();
    }

    public void StartWithoutTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
