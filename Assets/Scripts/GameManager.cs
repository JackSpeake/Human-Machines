using System.Collections;
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
    [SerializeField] public int hp;

    [SerializeField] private TMPro.TMP_Text firedText, hackedText;
    [SerializeField] private GameObject restartPanel;

    [SerializeField] public NotificationArea notifications;
    [SerializeField] private GameObject clockOut, clockIn, dayBreakdown;
    [SerializeField] private int[] WeeklyRates;
    [SerializeField] private string[] promotionNames;
    [SerializeField] private MessageItemManager miManager;
    [SerializeField] private TimeoutModule tmModule;

    [SerializeField] private GameObject confetti, congrats, fakeCongrats, newAssignment, newAssignmentAfterColorChange, finalAssignment, finalAssignmentColorChange;

    [SerializeField] private ReblockedMessages reMsg;
    [SerializeField] private AudioSource shutdownSound;
    [SerializeField] private AudioSource promotionSound;

    [SerializeField] private Image newspaper, mainBackdrop;

    [SerializeField] private TMPro.TMP_Text[] creditTexts;

    [SerializeField] private GameObject circle;

    private bool lost = false;

    public bool started = false;
    public bool tutorial = false;

    bool halfDay = false;
    bool dayAlmostOver = false;
    bool dayOver = false;

    public bool hackLose = false;

    public int currMessages = 0;

    // This is lowkey bad design but it will make this shit SOOOOOO much easier
    [SerializeField] private GameObject messagePanel, consoleCommandsPanel, clockPanel, controlButtonsPanel, moduleControlPanel, notificationPanel, screenHeaders;
    [SerializeField] private MessageItem tutorialMessageItem, tutorialMessageGood, tutorialMessageBad;

    [TextArea]
    [SerializeField] private string[] halfwayDoneMessages, almostDoneMessages, finishUpMessages, goodbyeMessages, welcomeBackMessages;


    private static GameManager _instance;

    private Flags[] dayFlags = { Flags.DayOneCompleted, Flags.DayTwoCompleted, Flags.DayThreeCompleted, Flags.DayFourCompleted, Flags.DayFiveCompleted, Flags.DaySixCompleted, Flags.DaySevenCompleted, Flags.DayEightCompleted, Flags.DayNineCompleted, Flags.DayTenCompleted, Flags.DayElevenCompleted, Flags.DayTwelveCompleted, Flags.DayThriteenCompleted, Flags.DayFourteenCompleted };

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

        reMsg.blocked_messages.Clear();
        reMsg.num_pages = 0;
        reMsg.num_pages = 0;


        if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
        {
            tutorial = true;
        }

        //shutdownSound = GetComponentInChildren<AudioSource>();

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

            if (time > lengthOfDay / 2 && !halfDay)
            {
                SendNotification(chooseRandomString(halfwayDoneMessages));
                halfDay = true;
            }

            if (time > lengthOfDay - (lengthOfDay / 8) && !dayAlmostOver)
            {
                SendNotification(chooseRandomString(almostDoneMessages));
                dayAlmostOver = true;
            }

            if (time > lengthOfDay)
            {
                NextDay();
            }
        }
        
    }

    void StartUp()
    {
        // audiosource.volume = 0.1f;
        // audiosource.pitch = 2.46f;
        // audiosource.reverbZoneMix = 0.99087f;
        // audiosource.clip = shutdownSound;

        miManager.Reset();
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
        shutdownSound.Play(0);

        yield return new WaitForSeconds(startUpBaseTime);

        notificationPanel.SetActive(true);
        shutdownSound.Play(0);

        // do thing
        if (tutorial)
        {
            SetTutorialMode(true);

            SendNotification("Welcome to Human Machines... This is your notification panel, press space to continue.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

            SendNotification("This panel is where you will recieve correspondance from your superiors.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

            SendNotification("It will only wait for you to continue during tutorials, so pay attention when you see the panel glow!");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

       

        messagePanel.SetActive(true);
        shutdownSound.Play();

        // do thing
        if (tutorial)
        {
            SendNotification("This is the message panel, it is where you will recieve dns requests from users.");
            SendNotification("You will need to accept or decline these requests in order to protect our users from harm.");

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

            SendNotification("As an employee of Human Machines, your job is to screen incoming requests, judging the good from the bad");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

            SendNotification("Here is an example of a good message");
            m.SpawnMessage(tutorialMessageGood);
            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

            SendNotification("And here is an example of a bad message");
            m.SpawnMessage(tutorialMessageBad);
            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

            SendNotification("You can read more about your current client and your goals for them in the handbook.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();

        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        consoleCommandsPanel.SetActive(true);
        shutdownSound.Play();

        // do thing
        if (tutorial)
        {
            SendNotification("This is the console, it doesn't do anything... Probably.");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(startUpBaseTime);
        }

        clockPanel.SetActive(true);
        shutdownSound.Play();

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
        shutdownSound.Play();

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
        shutdownSound.Play();

        // do thing
        if (tutorial)
        {
            SendNotification("The shop, exit and handbook can be accessed from these buttons.");

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

            SetTutorialMode(false);
        }
        else
        {
            SetTutorialMode(false);

            SendNotification("Welcome back to Human Machines. Your shift starts now, USER NUMBER " + Random.Range(10000, 99999).ToString() + ".");

            while (!notifications.isDone())
                yield return new WaitForEndOfFrame();
        }

        PlayerPrefs.SetInt("Tutorial", 0);
        started = true;
    }

    // Runs if hp hits 0. If it runs, begin lose coroutine
    public void Lose()
    {
        miManager.Reset();

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
            shutdownSound.Play();
            yield return new WaitForSeconds(newTime);
            newTime *= loseDestroySpeedupRate;
        }

        yield return new WaitForSeconds(loseDestroyBaseTime);

        if (hackLose)
            hackedText.enabled = true;
        else
            firedText.enabled = true;

        shutdownSound.Play();

        yield return new WaitForSeconds(loseDestroyBaseTime * 4);

        if (hackLose)
            hackedText.enabled = false;
        else
            firedText.enabled = false;

        shutdownSound.Play();

        yield return new WaitForSeconds(loseDestroyBaseTime * 3);

        SetFlags.ResetFlags();

        restartPanel.SetActive(true);

        shutdownSound.Play();

        hackLose = false;

        //Destroy(spawner);
    }

    // Iterates to next day. Basically no functionality
    void NextDay()
    {
        SendNotification(chooseRandomString(finishUpMessages));
        messagePanel.GetComponentInChildren<MessageSpawner>().spawning = false;
        
        started = false;
        StartCoroutine(waitForEndOfDay());
        //day++;

        // dayAlmostOver = false;
        // halfDay = false;
        // time = 0;
    }

    IEnumerator waitForEndOfDay()
    {
        while (currMessages > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        SendNotification(chooseRandomString(goodbyeMessages));

        while (!notifications.isDone())
        {
            yield return new WaitForEndOfFrame();
        }

        GameObject[] panels = GameObject.FindGameObjectsWithTag("Module");

        float newTime = loseDestroyBaseTime;

        foreach (GameObject g in panels)
        {
            g.SetActive(false);
            shutdownSound.Play();
            yield return new WaitForSeconds(newTime);
            newTime *= loseDestroySpeedupRate;
        }

        yield return new WaitForSeconds(.5f);

        shutdownSound.Play();
        clockOut.SetActive(true);
    }

    public void ClockOut()
    {
        StartCoroutine(DayBreakdown());
    }

    public void ClockIn()
    {
        StartCoroutine(StartNewNotFirstDay());
    }

    // this is bad code, dont look
    // ALSO IMPORTANT THERE ARE A LOT OF RANDOM FLOATS IN HERE THAT NEED TO BE MADE INTO VARIABLES FOR LATER TWEAKING!!!!
    IEnumerator DayBreakdown()
    {
        clockOut.SetActive(false);
        shutdownSound.Play();

        yield return new WaitForSeconds(.5f);


        dayBreakdown.SetActive(true);


        // Everything from here down is REALLY bad code
        // I am sorry ;;
        TMPro.TMP_Text headerText, lowerText;

        headerText = dayBreakdown.GetComponentInChildren<TMPro.TMP_Text>();
        List<TMPro.TMP_Text> texts = new List<TMPro.TMP_Text>(headerText.gameObject.GetComponentsInChildren<TMPro.TMP_Text>());
        texts.Remove(headerText);
        lowerText = texts[0];
        if (GameObject.Find("SoundManager") != null)
        {
            GameObject.Find("SoundManager").GetComponent<SoundController>().headerText = headerText;
            GameObject.Find("SoundManager").GetComponent<SoundController>().lowerText = lowerText;
        }

        int profit = (int)((float)WeeklyRates[stage - 1] * (DayBreakdownClass.GetPercent() / 100f));
        money += profit;
        DayBreakdownClass.moneyEarned = profit;

        headerText.text = DayBreakdownClass.GetHeaderString();
        lowerText.text = DayBreakdownClass.GetBreakdownString();

        headerText.maxVisibleCharacters = 0;
        lowerText.maxVisibleCharacters = 0;

        while (headerText.maxVisibleCharacters < headerText.text.Length)
        {
            headerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);

        while (lowerText.maxVisibleCharacters < lowerText.text.Length)
        {
            lowerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.03f);
        }

        yield return new WaitForSeconds(3f);

        while (lowerText.maxVisibleCharacters > 0)
        {
            lowerText.maxVisibleCharacters--;
            yield return new WaitForSeconds(.008f);
        }

        while (headerText.maxVisibleCharacters > 0)
        {
            headerText.maxVisibleCharacters--;
            yield return new WaitForSeconds(.008f);
        }

        if (GameObject.Find("SoundManager") != null)
        {
            GameObject.Find("SoundManager").GetComponent<SoundController>().headerText = null;
            GameObject.Find("SoundManager").GetComponent<SoundController>().lowerText = null;
        }

        yield return new WaitForSeconds(.5f);

        DayBreakdownClass.Reset();
        dayBreakdown.SetActive(false);
        
        yield return new WaitForSeconds(.5f);

        if (++day > daysInStage)
        {
            day = 1;
            stage++;
            messagePanel.GetComponentInChildren<MessageSpawner>().ResetMessages();
            if (stage < 4)
                StartCoroutine(StartNewWeek());
            else
                StartCoroutine(GameEnd());
        }
        else
        {
            clockIn.SetActive(true);
            shutdownSound.Play();
        }

        hp += 40;

        //
        SetFlags.addFlag(dayFlags[(day - 2) + ((stage - 1) * 5)]);


    }


    // I copy pasted a lot of this oopsies i def should put them in their own class zzz
    IEnumerator StartNewWeek()
    {
        yield return new WaitForEndOfFrame();

        congrats.SetActive(true);

        TMPro.TMP_Text headerText, lowerText;

        headerText = congrats.GetComponentInChildren<TMPro.TMP_Text>();
        List<TMPro.TMP_Text> texts = new List<TMPro.TMP_Text>(headerText.gameObject.GetComponentsInChildren<TMPro.TMP_Text>());
        texts.Remove(headerText);
        lowerText = texts[0];

        headerText.maxVisibleCharacters = 0;
        lowerText.maxVisibleCharacters = 0;

        while (headerText.maxVisibleCharacters < headerText.text.Length)
        {
            headerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);

        while (lowerText.maxVisibleCharacters < lowerText.text.Length)
        {
            lowerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.03f);
        }

        yield return new WaitForSeconds(1.5f);

        promotionSound.Play(0);
        confetti.SetActive(true);
        congrats.SetActive(false);
        newAssignment.SetActive(true);

        yield return new WaitForSeconds(7.5f);

        newAssignment.SetActive(false);
        newAssignmentAfterColorChange.SetActive(true);
       

        TMPro.TMP_Text text = newAssignmentAfterColorChange.GetComponentInChildren<TMPro.TMP_Text>();

        texts = new List<TMPro.TMP_Text>(text.gameObject.GetComponentsInChildren<TMPro.TMP_Text>());
        texts.Remove(text);
        lowerText = texts[0];

        text.color = Color.red;
        lowerText.color = Color.red;

        yield return new WaitForSeconds(1f);

        lowerText.text += "NEW ASSIGNMENT:";

        yield return new WaitForSeconds(1f);

        lowerText.text += "\n" + promotionNames[stage-2];

        yield return new WaitForSeconds(3f);

        lowerText.text = "";

        confetti.SetActive(false);
        newAssignmentAfterColorChange.SetActive(false);
        clockIn.SetActive(true);
    }

    // also not amazing code but hopefully it works
    IEnumerator StartNewNotFirstDay()
    {
        clockIn.SetActive(false);

        time = 0;

        if (day > 1)
            SetFlags.addFlag(Flags.DayOneCompleted);

        yield return new WaitForSeconds(.5f);

        GameObject[] panels = { screenHeaders, messagePanel, consoleCommandsPanel, clockPanel, controlButtonsPanel, moduleControlPanel, notificationPanel };

        float newTime = loseDestroyBaseTime;

        foreach (GameObject g in panels)
        {
            g.SetActive(true);
            shutdownSound.Play();
            yield return new WaitForSeconds(newTime);
            newTime *= loseDestroySpeedupRate;
        }

        dayAlmostOver = false;
        halfDay = false;
        SendNotification(chooseRandomString(welcomeBackMessages));

        while (!notifications.isDone())
        {
            yield return new WaitForEndOfFrame();
        }

        started = true;
        messagePanel.GetComponentInChildren<MessageSpawner>().spawning = true;
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
        //Application.Quit();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Reloads the scene, aka START OVER
    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    // This doesn't technically need to be here, but it will make it easier to send notifications to the notification zone
    // because the gamemanager is a singleton so anyone can access this method from anywhere.
    public void SendNotification(string m)
    {
        if (!tmModule.paused) {
            notifications.showMessage(m);
        }
        
    }

    public void SendEvilNotification(string m)
    {
        if (!tmModule.paused)
        {
            notifications.showMessage(m, true);
        }

    }

    public void SetTutorialMode(bool mode)
    {
        notifications.SetTutorialMode(mode);
    }

    public bool GetNotificationStatus()
    {
        return notifications.displaying;
    }

    public void SendCustomYap(YapperState yap, float time)
    {
        if (!tmModule.paused) {
            notifications.setCustomYapper(yap, time);
        }
    }

    public string chooseRandomString(string[] strs)
    {
        return strs[Random.Range(0, strs.Length)];
    }


    private IEnumerator GameEnd()
    {
        yield return new WaitForEndOfFrame();

        circle.SetActive(false);

        fakeCongrats.SetActive(true);

        TMPro.TMP_Text headerText, lowerText;

        headerText = fakeCongrats.GetComponentInChildren<TMPro.TMP_Text>();
        List<TMPro.TMP_Text> texts = new List<TMPro.TMP_Text>(headerText.gameObject.GetComponentsInChildren<TMPro.TMP_Text>());
        texts.Remove(headerText);
        lowerText = texts[0];

        headerText.maxVisibleCharacters = 0;
        lowerText.maxVisibleCharacters = 0;

        while (headerText.maxVisibleCharacters < headerText.text.Length)
        {
            headerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);

        while (lowerText.maxVisibleCharacters < lowerText.text.Length)
        {
            lowerText.maxVisibleCharacters++;
            yield return new WaitForSeconds(.03f);
        }

        yield return new WaitForSeconds(1.5f);

        promotionSound.Play(0);
        confetti.SetActive(true);
        fakeCongrats.SetActive(false);
        finalAssignment.SetActive(true);

        yield return new WaitForSeconds(7.5f);

        finalAssignment.SetActive(false);
        finalAssignmentColorChange.SetActive(true);


        TMPro.TMP_Text text = finalAssignmentColorChange.GetComponentInChildren<TMPro.TMP_Text>();

        texts = new List<TMPro.TMP_Text>(text.gameObject.GetComponentsInChildren<TMPro.TMP_Text>());
        texts.Remove(text);
        lowerText = texts[0];

        text.color = Color.red;
        lowerText.color = Color.red;

        yield return new WaitForSeconds(1f);

        lowerText.text += "NEW ASSIGNMENT:";

        yield return new WaitForSeconds(1f);

        lowerText.text += "\n" + "GET OUT";

        yield return new WaitForSeconds(3f);

        lowerText.text = "";

        confetti.SetActive(false);
        finalAssignmentColorChange.SetActive(false);

        // Newspaper here
        yield return new WaitForSeconds(1f);

        float d = 0;
        while (d < 1f)
        {
            mainBackdrop.rectTransform.localScale = new Vector3(1 - d, 1 - d, 1 - d);
            d += Time.deltaTime * 6f;
            yield return new WaitForEndOfFrame();
        }

        mainBackdrop.rectTransform.localScale = new Vector3(0, 0, 0);
        mainBackdrop.enabled = false;

        yield return new WaitForSeconds(4f);

        d = 0;
        newspaper.gameObject.SetActive(true);
        while (d < 3f)
        {
            newspaper.color = new Color(1, 1, 1, d / 3);
            d += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(10f);

        d = 3f;
        while (d > 0)
        {
            newspaper.color = new Color(1, 1, 1, d / 3);
            d -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newspaper.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        StartCoroutine(CreditsCoroutine());
    }


    private IEnumerator CreditsCoroutine()
    {
        yield return new WaitForEndOfFrame();

        creditTexts[0].gameObject.SetActive(true);
        creditTexts[1].gameObject.SetActive(true);
        creditTexts[0].maxVisibleCharacters = 0;
        creditTexts[1].maxVisibleCharacters = 0;

        while (creditTexts[0].maxVisibleCharacters < creditTexts[0].text.Length)
        {
            creditTexts[0].maxVisibleCharacters++;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(1f);

        while (creditTexts[1].maxVisibleCharacters < creditTexts[1].text.Length)
        {
            creditTexts[1].maxVisibleCharacters++;
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(2f);

        while (creditTexts[1].maxVisibleCharacters > 0)
        {
            creditTexts[1].maxVisibleCharacters--;
            yield return new WaitForSeconds(.03f);
        }

        while (creditTexts[0].maxVisibleCharacters > 0)
        {
            creditTexts[0].maxVisibleCharacters--;
            yield return new WaitForSeconds(.03f);
        }

        creditTexts[0].gameObject.SetActive(false);
        creditTexts[1].gameObject.SetActive(false);

        for (int i = 2; i < 9; i++)
        {
            yield return new WaitForSeconds(1.5f);

            creditTexts[i].gameObject.SetActive(true);
            creditTexts[i].maxVisibleCharacters = 0;

            while (creditTexts[i].maxVisibleCharacters < creditTexts[i].text.Length)
            {
                creditTexts[i].maxVisibleCharacters++;
                yield return new WaitForSeconds(.1f);
            }

            yield return new WaitForSeconds(2f);

            while (creditTexts[i].maxVisibleCharacters > 0)
            {
                creditTexts[i].maxVisibleCharacters--;
                yield return new WaitForSeconds(.03f);
            }

            creditTexts[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
