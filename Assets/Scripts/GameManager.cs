using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int day = 1;
    public int stage = 1;
    [SerializeField] private int daysInStage;

    [SerializeField] private float loseDestroyBaseTime;
    [SerializeField] private float loseDestroySpeedupRate;

    // In seconds
    public float time = 0;
    // In seconds
    [SerializeField] public float lengthOfDay = 64800;
    [SerializeField] public float daySpeedRatio = 20;

    [SerializeField] private int maxHP = 100;
    [SerializeField] private int hp;

    [SerializeField] private TMPro.TMP_Text firedText;
    [SerializeField] private GameObject restartPanel;

    [SerializeField] private NotificationArea notifications;

    private bool lost = false;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    // FOR SINGLETON. DO NOT DELETE.
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    // Checks hp and time to check if we need to progress game.
    void Update()
    {
        time += Time.deltaTime * daySpeedRatio;

        if (hp <= 0)
        {
            Lose();
        }

        if (time > lengthOfDay)
        {
            NextDay();
        }

        if (day > daysInStage)
        {
            NextStage();
        }
    }

    // Runs if hp hits 0. If it runs, begin lose coroutine
    void Lose()
    {
        if (!lost)
        {
            lost = true;

            StartCoroutine(loseCutscene());
        }
    }

    // Deletes each part of the screen one piece at a time
    // Then lets the user restart the game.
    private IEnumerator loseCutscene()
    {
        GameObject[] messages = GameObject.FindGameObjectsWithTag("Message");
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Module");
        GameObject spawner = GameObject.FindGameObjectWithTag("Spawner");

        // stop spawning
        spawner.GetComponent<MessageSpawner>().spawning = false;

        float newTime = loseDestroyBaseTime;

        yield return new WaitForSeconds(loseDestroyBaseTime);

        foreach (GameObject g in messages)
        {
            Destroy(g);
            yield return new WaitForSeconds(newTime);
            newTime *= loseDestroySpeedupRate;
        }

        newTime = loseDestroyBaseTime;
        yield return new WaitForSeconds(newTime);

        foreach (GameObject g in panels)
        {
            Destroy(g);
            yield return new WaitForSeconds(newTime);
            newTime *= loseDestroySpeedupRate;
        }

        yield return new WaitForSeconds(loseDestroyBaseTime);

        firedText.enabled = true;

        yield return new WaitForSeconds(loseDestroyBaseTime * 4);

        firedText.enabled = false;

        yield return new WaitForSeconds(loseDestroyBaseTime * 3);

        restartPanel.SetActive(true);

        //Destroy(spawner);
    }

    // Iterates to next day. Basically no functionality
    void NextDay()
    {
        time = 0;
        day++;
    }

    // Iterates to next stage. Basically no functionality
    void NextStage()
    {
        day = 1;
        stage++;
    }

    // Lets other scripts do damage to the player
    public void takeDamage(int dmg)
    {
        hp -= dmg;
    }

    // Exits the game
    public void DontRetry()
    {
        Application.Quit();
    }

    // Reloads the scene, aka START OVER
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    // This doesn't technically need to be here, but it will make it easier to send notifications to the notification zone
    // because the gamemanager is a singleton so anyone can access this method from anywhere.
    public void SendNotification(string m)
    {
        notifications.showMessage(m);
    }

}
