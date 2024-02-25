using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MessageObject.MessageState;


public class Reblocker : MonoBehaviour
{
    [SerializeField] private GameObject messageArea;
    [SerializeField] private float deletionTime;
    [SerializeField] private TMPro.TMP_Text moduleText;
    public bool isActive = false;
    public List<string> blocked_messages = new List<string>();
    public MessageObject[] messagesOnScreen;

    // Start is called before the first frame update
    void Start()
    {
        moduleText.text = "--- Reblocker Module ---\n Blocked Messages:\n\n";
        isActive = false;
        blocked_messages.Clear();
        //blocked_messages.Add("Request Recieved: NOTAVIRUS.virus");
    }

    // Update is called once per frame
    void Update()
    {
        messagesOnScreen = messageArea.GetComponentsInChildren<MessageObject>();

        foreach (MessageObject msg in messagesOnScreen) {
            //Debug.Log(msg.GetMessageText());
            // if (blocked_messages.Contains(msg.GetMessageText()) && msg.state != MessageObject.MessageState.Reblocked) {
            //     Debug.Log("HIT");
            if (msg.GetMessageItem().reblocked && !blocked_messages.Contains(msg.GetMessageText())) {
                blocked_messages.Add(msg.GetMessageText());
                moduleText.text = moduleText.text + "> " + msg.GetMessageText() + "\n";
            }
            
            if ((blocked_messages.Contains(msg.GetMessageText()) && msg.state != MessageObject.MessageState.Reblocked) ) {
                msg.state = MessageObject.MessageState.Reblocked;
                StartCoroutine(Reblock(msg));
            }
                
                //StartCoroutine(Reblock(msg));
            //}
        }
    }

    public void BlockMessage(TMPro.TMP_Text msgItem) {
        Debug.Log("Hit");
        string msg = msgItem.text;
        if (!blocked_messages.Contains(msg)) {
            blocked_messages.Add(msg);
        }
    }

    bool CanReblock(string msg) {
        foreach (string block in blocked_messages) {
            Debug.Log("Tried Comparison with ---" + block + "--- and ---" + msg + "---\n");
            if (block.Equals(msg)) {
                return true;
            }
        }
        return false;
    }

    public void Activate() {
        isActive = true;
    }

    private IEnumerator Reblock(MessageObject msg) {
        float t = deletionTime;
        msg.SetMessageText("----Reblocked----");

        while (t > 0.0f)
        {
            //Color retColor = new Color(1.0f, t/deletionTime, t/deletionTime, t/deletionTime);
            // Color retColor = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 0.0f, 0.0f, 0.0f), t / deletionTime);
            // msg.ChangeMessageColor(retColor);
            t -= Time.deltaTime;            
        }
        yield return new WaitForSeconds(deletionTime);
        
        if (msg != null) {
            msg.Decline();    
        }     
        
    }



}
