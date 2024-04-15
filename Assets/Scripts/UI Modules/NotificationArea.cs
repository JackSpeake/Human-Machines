using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationArea : MonoBehaviour
{
    [SerializeField] public TMPro.TMP_Text messageText, animationText;
    [SerializeField] private float displayRate;
    [SerializeField] private float displayRateYapperAnim;
    [SerializeField] private float randomYapperChangeTime;
    [SerializeField] private float timeBetweenRandomYappersLow, timeBetweenRandomYappersHigh;
    [SerializeField] private float stayTimeBeforeNextMessage;

    [SerializeField] private YapperState[] otherYappers;
    [SerializeField] private YapperState defaultYapper, openMouthYapper, defaultEvilYapper, openMouthEvilYapper;

    [SerializeField] private GlowLerp tutorialGlow;

    public bool tutorial = false;

    bool pressingSpace = false;

    public bool displaying = false;
    private bool animating = false;
    private Queue<string> stringQueue;
    private Queue<bool> evilQueue;
    private bool customYap = false;

    bool currentlyEvil = false;

    float t = 0;
    float randTime = 0;
    bool inRandYap = false;


    private void Start()
    {
        stringQueue = new Queue<string>();
        evilQueue = new Queue<bool>();

        if (GameObject.Find("SoundManager") != null)
            GameObject.Find("SoundManager").GetComponent<SoundController>().notificationArea = this;
    }

    public bool isDone()
    {
        return !(displaying || animating);
    }

    public void setCustomYapper(YapperState yap, float time)
    {
        customYap = true;
        animationText.text = yap.yapImg;

        Invoke("endCustomYap", time);
    }

    private void endCustomYap()
    {
        customYap = false;
    }

    // Checks if we are displaying, if we arent, display next from the queue
    private void Update()
    {
        t += Time.unscaledDeltaTime;


        if (tutorial && Input.GetKeyDown(KeyCode.Space))
            pressingSpace = true;
        else
            pressingSpace = false;

        if (currentlyEvil)
            animationText.color = Color.red;
        else
            animationText.color = Color.white;

        if (!displaying && stringQueue.Count != 0)
        {
            showMessage(stringQueue.Dequeue(), evilQueue.Dequeue());
        }

        if (!animating && !customYap)
        {
            // MAKE THIS RANDOM YAP
            if (randTime == 0 && !inRandYap)
            {
                randTime = Random.Range(timeBetweenRandomYappersLow, timeBetweenRandomYappersHigh);
            }
            else if (t > randTime && !inRandYap)
            {
                t = 0;
                randTime = 0;
                inRandYap = true;
                animationText.text = otherYappers[Random.Range(0, otherYappers.Length - 1)].yapImg;
            }
            else if (inRandYap)
            {
                if (t > randomYapperChangeTime)
                {
                    animationText.text = defaultYapper.yapImg;
                    t = 0;
                    inRandYap = false;
                }
            }
            else
            {
                animationText.text = defaultYapper.yapImg;
            }  
        }
    }

    // Begins the message animation if not animating, if animating, puts the message into the queue
    public void showMessage(string message)
    {
        if (displaying)
        {
            stringQueue.Enqueue(message);
            evilQueue.Enqueue(false);
        }
        else
        {
            displaying = true;
            StartCoroutine(displayMessageOverTime(message, false));
            if (!animating)
            {
                StartCoroutine(animateFace());
            }
        }
    }

    // Begins the message animation if not animating, if animating, puts the message into the queue
    public void showMessage(string message, bool evil)
    {
        if (displaying)
        {
            stringQueue.Enqueue(message);
            evilQueue.Enqueue(evil);
        }
        else
        {
            displaying = true;
            StartCoroutine(displayMessageOverTime(message, evil));
            if (!animating)
            {
                StartCoroutine(animateFace());
            }
        }
    }

    // Coroutine that displays the message one character at a time
    private IEnumerator displayMessageOverTime(string message, bool evil)
    {
        messageText.text = message;
        messageText.maxVisibleCharacters = 0;
        messageText.linkedTextComponent.maxVisibleCharacters = 0;

        if (evil)
        {
            messageText.color = Color.red;
            currentlyEvil = true;
        }
        else
            currentlyEvil = false;


        while (messageText.maxVisibleCharacters < messageText.text.Length)
        {
            messageText.maxVisibleCharacters++;
           
            if (!messageText.isTextOverflowing || !(messageText.maxVisibleCharacters > 50 && messageText.maxVisibleCharacters >= messageText.firstOverflowCharacterIndex - 1))
                yield return new WaitForSeconds(displayRate);
        }

        messageText.linkedTextComponent.maxVisibleCharacters = messageText.firstOverflowCharacterIndex;

        while (messageText.linkedTextComponent.maxVisibleCharacters < messageText.linkedTextComponent.text.Length)
        {
            messageText.linkedTextComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(displayRate);
        }

        if (tutorial)
        {
            messageText.text = messageText.text + "█";
            bool backAndForth = true;
            float t = 0;

            while (tutorial)
            {
                
                t += Time.unscaledDeltaTime;
                if (backAndForth && t > .5f)
                {
                    messageText.linkedTextComponent.maxVisibleCharacters++;
                    messageText.maxVisibleCharacters++;
                    backAndForth = false;
                    t = 0;
                }
                else if (t > .5f)
                {
                    messageText.linkedTextComponent.maxVisibleCharacters--;
                    messageText.maxVisibleCharacters--;
                    backAndForth = true;
                    t = 0;
                }

                if (pressingSpace)
                    break;

                yield return new WaitForEndOfFrame();

            }
        }
        else
        {
            yield return new WaitForSeconds(stayTimeBeforeNextMessage);
        }

        messageText.text = "";

        messageText.color = Color.white;
        currentlyEvil = false;
        displaying = false;
    }

    // Animates the face by swapping between the two strings
    private IEnumerator animateFace()
    {
        animating = true;
        animationText.text = "";

        
        while (displaying)
        {

            if (currentlyEvil)
            {
                if (animationText.text != openMouthEvilYapper.yapImg)
                {
                    animationText.text = openMouthEvilYapper.yapImg;
                }
                else
                {
                    animationText.text = defaultEvilYapper.yapImg;
                }
            }
            else
            {
                if (animationText.text != openMouthYapper.yapImg)
                {
                    animationText.text = openMouthYapper.yapImg;
                }
                else
                {
                    animationText.text = defaultYapper.yapImg;
                }
            }

            
            yield return new WaitForSeconds(displayRate * 3);
        }

        animationText.text = defaultYapper.yapImg;
        animating = false;
    }

    public void SetTutorialMode(bool mode)
    {
        tutorial = mode;
        tutorialGlow.ToggleGlow(mode);
    }

}
