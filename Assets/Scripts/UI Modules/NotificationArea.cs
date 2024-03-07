using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationArea : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText, animationText;
    [SerializeField] private float displayRate;
    [SerializeField] private float displayRateYapperAnim;
    [SerializeField] private float randomYapperChangeTime;
    [SerializeField] private float timeBetweenRandomYappersLow, timeBetweenRandomYappersHigh;
    [SerializeField] private float stayTimeBeforeNextMessage;

    [SerializeField] private YapperState[] otherYappers;
    [SerializeField] private YapperState defaultYapper, openMouthYapper;

    private bool displaying = false;
    private bool animating = false;
    private Queue<string> stringQueue;
    private bool customYap = false;

    float t = 0;
    float randTime = 0;
    bool inRandYap = false;


    private void Start()
    {
        stringQueue = new Queue<string>();

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
        t += Time.deltaTime;

        if (!displaying && stringQueue.Count != 0)
        {
            showMessage(stringQueue.Dequeue());
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
        }
        else
        {
            displaying = true;
            StartCoroutine(displayMessageOverTime(message));
            if (!animating)
            {
                StartCoroutine(animateFace());
            }
        }
    }

    // Coroutine that displays the message one character at a time
    private IEnumerator displayMessageOverTime(string message)
    {
        messageText.text = message;
        messageText.maxVisibleCharacters = 0;
        messageText.linkedTextComponent.maxVisibleCharacters = 0;


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

        yield return new WaitForSeconds(stayTimeBeforeNextMessage);
        messageText.text = "";
        displaying = false;
    }

    // Animates the face by swapping between the two strings
    private IEnumerator animateFace()
    {
        animating = true;
        animationText.text = "";

        while (displaying)
        {
            if (animationText.text != openMouthYapper.yapImg)
            {
                animationText.text = openMouthYapper.yapImg;
            }
            else
            {
                animationText.text = defaultYapper.yapImg;
            }
            yield return new WaitForSeconds(displayRate * 3);
        }

        animationText.text = defaultYapper.yapImg;
        animating = false;
    }

}
