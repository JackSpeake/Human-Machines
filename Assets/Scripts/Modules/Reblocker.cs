using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MessageObject.MessageState;


public class Reblocker : MonoBehaviour
{
    [SerializeField] private GameObject messageArea;
    [SerializeField] private float deletionTime;
    [SerializeField] private TMPro.TMP_Text moduleText;
    [SerializeField] private ReblockedMessages reMsg;
    public bool isActive = false;
    // public List<List<string>> blocked_messages = new List<List<string>>();
    public MessageObject[] messagesOnScreen;


    // [SerializeField] private int num_pages = 1;
    // [SerializeField] private int current_page = 1;

    // Start is called before the first frame update
    void Start()
    {
        moduleText.text = "Reblocker Module - Page 1\n Blocked Messages:\n\n";
        isActive = false;
        reMsg.blocked_messages.Clear();
        reMsg.blocked_messages.Add(new List<string>());
        reMsg.num_pages = 1;
        reMsg.current_page = 1;
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
            // if (!blocked_messages[num_pages - 1].Contains(msg.GetMessageText()) && blocked_messages[num_pages - 1].Count <= 5) {
            //     blocked_messages[num_pages - 1].Add(msg.GetMessageText());
            //     moduleText.text = moduleText.text + "> " + msg.GetMessageText() + "\n";
            // }
            UpdateReblockerText();
            
            if ((CanReblock(msg.GetMessageText())) ) {
                StartCoroutine(Reblock(msg));
            }
                
                //StartCoroutine(Reblock(msg));
            //}
        }
    }

    // public void BlockMessage(TMPro.TMP_Text msgItem) {
    //     Debug.Log("Hit");
    //     string msg = msgItem.text;
    //     if (!blocked_messages.Contains(msg)) {
    //         blocked_messages.Add(msg);
    //     }
    // }

    bool CanReblock(string msg) {
        foreach (List<string> block in reMsg.blocked_messages) {
            foreach (string blocked in block) {
                if (blocked.Equals(msg)) {
                    return true;
                }
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

    public void PageUp() {
        reMsg.current_page = Mathf.Clamp(reMsg.current_page + 1, 1, reMsg.num_pages);
        UpdateReblockerText();
    }

    public void PageDown() {
        reMsg.current_page = Mathf.Clamp(reMsg.current_page - 1, 1, reMsg.num_pages);
        UpdateReblockerText();
    }

    private void UpdateReblockerText() {
        moduleText.text = "Reblocker Module - Page " + reMsg.current_page + "\n Blocked Messages:\n\n";
        foreach (string msg in reMsg.blocked_messages[reMsg.current_page - 1]) {
            moduleText.text = moduleText.text + "> " + msg + "\n";
        }
    }



}
