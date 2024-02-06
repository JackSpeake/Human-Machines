using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationArea : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText, animationText;
    [SerializeField] private float displayRate;
    [SerializeField] private float stayTimeBeforeNextMessage;
    [TextArea]
    [SerializeField] private string openMouth, closeMouth;


    private bool displaying = false;
    private bool animating = false;
    private Queue<string> stringQueue;



    private void Start()
    {
        stringQueue = new Queue<string>();
    }

    private void Update()
    {
        if (!displaying && stringQueue.Count != 0)
        {
            showMessage(stringQueue.Dequeue());
        }
    }

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

    private IEnumerator displayMessageOverTime(string message)
    {
        messageText.text = "";
        foreach (char c in message)
        {
            messageText.text += c;
            yield return new WaitForSeconds(displayRate);
        }

        yield return new WaitForSeconds(stayTimeBeforeNextMessage);
        messageText.text = "";
        displaying = false;
    }

    private IEnumerator animateFace()
    {
        animating = true;
        animationText.text = "";

        while (displaying)
        {
            if (animationText.text != openMouth)
            {
                animationText.text = openMouth;
            }
            else
            {
                animationText.text = closeMouth;
            }
            yield return new WaitForSeconds(displayRate * 3);
        }

        animationText.text = "";
        animating = false;
    }

}
