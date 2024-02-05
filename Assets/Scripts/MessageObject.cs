using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text messageText;
    [SerializeField] private Button acceptButton, declineButton;
    [SerializeField] private MessageItem messageItem;

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
        //messageText.enabled = true;
        //acceptButton.enabled = true;
        //declineButton.enabled = true;
    }

    // theyre the same for now lmao
    // This should be where the reaction to accepting / denying should happen
    public void Accept()
    {
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

