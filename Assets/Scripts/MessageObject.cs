using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText;
    [SerializeField] private Button acceptButton, declineButton;
    [SerializeField] private MessageItem messageItem;
    [SerializeField] private float jitSpeedInc, jitAngleInc, jitCurveMult;

    private TMPro.Examples.VertexJitter jitterComp;
    private bool completed = false;

    private static bool firstAccept = false;
    private static bool firstBeginTimeout = false;

    [TextArea]
    [SerializeField] private string[] failedToAnswerMessages;

    private void Start()
    {
        GameManager.Instance.currMessages++;
        //messageText.enabled = false;
        //acceptButton.enabled = false;
        //declineButton.enabled = false;
    }

    // Used to set the message item of the object when the object is created
    public MessageObject(MessageItem m)
    {
        SetMessageItem(m);
    }

    public void SetMessageItem(MessageItem m)
    {
        messageItem = m;
        Startup();
    }

    private void Startup()
    {
        messageText.text = messageItem.message;
        messageText.maxVisibleCharacters = 0;
        acceptButton.gameObject.SetActive(false);
        declineButton.gameObject.SetActive(false);
        StartCoroutine(timeToKill());
        StartCoroutine(SpawnAnimation());
        jitterComp = messageText.GetComponent<TMPro.Examples.VertexJitter>();
    }

    private IEnumerator SpawnAnimation()
    {
        float letterTime = .01f;

        while (messageText.maxVisibleCharacters < messageText.text.Length)
        {
            messageText.maxVisibleCharacters++;

            yield return new WaitForSeconds(letterTime);
        }

        yield return new WaitForSeconds(letterTime * 4);

        acceptButton.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(letterTime * 4);

        declineButton.gameObject.SetActive(true);
    }

    /* I am unsure if running out of time on a message should
     *  a. Kill the message
     *  b. Just alert the user that its slow, but still have them respond to it
     *  c. Just let it through by default
     *  d. other?
     */


    private IEnumerator timeToKill()
    {

        yield return new WaitForSeconds(messageItem.timeToFail * (3f / 5f));

        if (!firstBeginTimeout)
        {
            GameManager.Instance.SendNotification("It's shaking! Answer before the request times out!");
            firstBeginTimeout = true;
        }
        
        //StartCoroutine(LerpColor(messageText.color, Color.clear, messageItem.timeToFail * (2f / 5f)));
        StartCoroutine(JitterRamp());
        

        yield return new WaitForSeconds(messageItem.timeToFail * (2f / 5f));

        if (!completed)
        {
            GameManager gm = GameManager.Instance;
            DayBreakdownClass.messagesMissed++;
            gm.SendNotification(gm.chooseRandomString(failedToAnswerMessages));
            gm.takeDamage(messageItem.failPoints);
            Destroy(this.gameObject);
        }
    }

    // DOES NOT WORK
    private IEnumerator LerpColor(Color startColor, Color endColor, float lerpTime)
    {
        float t = 0;

        while (t < lerpTime)
        {
            jitterComp.textColor = Color.Lerp(startColor, endColor, t / lerpTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator JitterRamp()
    {
        while (!completed)
        {
            jitterComp.AngleMultiplier += jitAngleInc;
            jitterComp.CurveScale += jitCurveMult;
            jitterComp.SpeedMultiplier += jitSpeedInc;
            yield return new WaitForSeconds(.1f);
        }
        
    }

    // theyre the same for now lmao
    // This should be where the reaction to accepting / denying should happen
    public void Accept()
    {
        if (!firstAccept)
        {
            GameManager.Instance.SendNotification("YOU JUST ACCEPTED A MESSAGE. WOW. GOOD JOB.");
            firstAccept = true;
        }

        if (!this.messageItem.correct && !this.messageItem.noFail)
        {
            GameManager.Instance.takeDamage(messageItem.failPoints);
            DayBreakdownClass.messagesIncorrect++;
        }
        else
        {
            DayBreakdownClass.messagesCorrect++;
        }

        if (this.messageItem.completeRaiseFlag)
        {
            SetFlags.addFlag(this.messageItem.acceptRaiseFlag);
        }

        if (this.messageItem.following)
        {
            if (this.messageItem.acceptFollowingMessage)
            {
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<MessageSpawner>()
                    .spawnOnFlagMessages.Add(this.messageItem.acceptFollowingMessage);
            }
        }

        Destroy(this.gameObject);
    }

    public void Decline()
    {
        if (this.messageItem.correct && !this.messageItem.noFail)
        {
            GameManager.Instance.takeDamage(messageItem.failPoints);
            DayBreakdownClass.messagesIncorrect++;
        }
        else
        {
            DayBreakdownClass.messagesCorrect++;
        }

        if (this.messageItem.completeRaiseFlag)
        {
            SetFlags.addFlag(this.messageItem.declineRaiseFlag);
        }
        if (this.messageItem.following)
        {
            if (this.messageItem.declineFollowingMessage)
            {
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<MessageSpawner>()
                    .spawnOnFlagMessages.Add(this.messageItem.declineFollowingMessage);
            }
        }

        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.currMessages--;
    }
}

