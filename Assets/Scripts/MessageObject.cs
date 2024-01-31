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

    public void Accept()
    {
        Destroy(this.gameObject);
    }

    public void Decline()
    {
        Destroy(this.gameObject);
    }
}

