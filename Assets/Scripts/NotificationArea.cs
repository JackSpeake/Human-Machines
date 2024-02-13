using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NotificationArea : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText, animationText;
    [SerializeField] private float displayRate;
    [SerializeField] private float stayTimeBeforeNextMessage;

    [SerializeField] private YapperState[] otherYappers;
    [SerializeField] private YapperState defaultYapper, openMouthYapper;

    private bool displaying = false;
    private bool animating = false;
    private Queue<string> stringQueue;


    private void Start()
    {
        stringQueue = new Queue<string>();

    }

    public bool isDone()
    {
        return !(displaying || animating);
    }

    // Checks if we are displaying, if we arent, display next from the queue
    private void Update()
    {
        if (!displaying && stringQueue.Count != 0)
        {
            showMessage(stringQueue.Dequeue());
        }

        if (!animating)
        {
            // MAKE THIS RANDOM YAP
            animationText.text = defaultYapper.yapImg;
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
