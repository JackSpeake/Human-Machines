using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText;
    [SerializeField] private Button acceptButton, declineButton;
    [SerializeField] private MessageItem messageItem;

    private bool completed = false;

    private void Start()
    {
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
        StartCoroutine(timeToKill());

    }

    /* I am unsure if running out of time on a message should
     *  a. Kill the message
     *  b. Just alert the user that its slow, but still have them respond to it
     *  c. Just let it through by default
     *  d. other?
     */


    private IEnumerator timeToKill()
    {
        yield return new WaitForSeconds(messageItem.timeToFail);

        if (!completed)
        {
            GameManager.Instance.takeDamage(messageItem.failPoints);
            Destroy(this.gameObject);
        }
    }

    // theyre the same for now lmao
    // This should be where the reaction to accepting / denying should happen
    public void Accept()
    {
        GameManager.Instance.SendNotification("YOU JUST ACCEPTED A MESSAGE. WOW. GOOD JOB.");

        if (!this.messageItem.correct)
        {
            GameManager.Instance.takeDamage(messageItem.failPoints);
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
        if (this.messageItem.correct)
        {
            GameManager.Instance.takeDamage(messageItem.failPoints);
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
}

