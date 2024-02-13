using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int day = 1;
    public int stage = 1;
    public int money = 100;
    [SerializeField] private int daysInStage;

    [SerializeField] private float loseDestroyBaseTime;
    [SerializeField] private float startUpBaseTime;
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

    public bool started = false;
    public bool tutorial = false;

    // This is lowkey bad design but it will make this shit SOOOOOO much easier
    [SerializeField] private GameObject messagePanel, consoleCommandsPanel, clockPanel, controlButtonsPanel, moduleControlPanel, notificationPanel, screenHeaders;
    [SerializeField] private MessageItem tutorialMessageItem;

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

        if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
        {
            tutorial = true;
        }

        StartUp();
    }

    // Update is called once per frame
    // Checks hp and time to check if we need to progress game.
    void Update()
    {
        if (started)
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
        
    }

    void StartUp()
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Module");

        foreach (GameObject g in panels)
        {
            g.SetActive(false);
        }

        StartCoroutine(startupCoroutine());
    }

    IEnumerator startupCoroutine()
    {
        yield return new WaitForSeconds(startUpBaseTime);

        screenHeaders.SetActive(true);

        yield return new WaitForSeconds(startUpBaseTime);

        notificationPanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("Welcome to Human Machines... This is your notification panel, where you will recieve correspondance from your superiors.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

       

        messagePanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("This is the message panel, it is where you will recieve dns requests from users. You will need to accept or decline these requests in order to protect our users from harm.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();


            SendNotification("Here is an example message, go ahead and accept it. Be warned, normal messages will fail after some time of indecision.");
            
            MessageSpawner m = messagePanel.GetComponentInChildren<MessageSpawner>();
            m.SpawnMessage(tutorialMessageItem);

            while(!SetFlags.activeFlags.Contains(Flags.tutorialAccepted))
            {       
                if (SetFlags.activeFlags.Contains(Flags.tutorialDeclined))
                {
                    SendNotification("I said to accept the message. If you can't follow instructions, you are not going to last long here. Try again.");

                    while (!notifications.isDone())
                        yield return new WaitForEndOfFrame();

                    SetFlags.removeFlag(Flags.tutorialDeclined);

                    m.SpawnMessage(tutorialMessageItem);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        consoleCommandsPanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("This is the console, you may access various commands from here.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        clockPanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("This is your shift clock, please ensure you complete your entire shift.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        moduleControlPanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("These are your modules, you don't have any yet.");
            SendNotification("Modules can be purchased in the shop, they will be vital in your success here.");
            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        controlButtonsPanel.SetActive(true);

        // do thing
        if (tutorial)
        {
            SendNotification("The shop, exit and credits can be accessed from these buttons.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        // do thing
        if (tutorial)
        {
            SendNotification("This concludes the tutorial, we are excited to see all of the great things you are going to accomplish! Good Luck.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            SendNotification("Welcome back to Human Machines. Your shift starts now, USER NUMBER " + Random.Range(10000, 99999).ToString() + ".");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }


        started = true;
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
