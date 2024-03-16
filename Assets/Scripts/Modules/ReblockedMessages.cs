using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ReblockedMessages", order = 1)]
public class ReblockedMessages : ScriptableObject
{
    public List<List<string>> blocked_messages = new List<List<string>>();
    public int num_pages = 1;
    public int current_page = 1;

    public void TryAdd(MessageObject msg) {
        if (msg.GetMessageItem().repeat) {
            string msgText = msg.GetMessageText();
            foreach (List<string> block in blocked_messages) {
                foreach (string blocked in block) {
                    if (blocked.Equals(msgText)) {
                        return;
                    }
                }
            }
            if (blocked_messages[current_page - 1].Count < 5) {
                blocked_messages[current_page - 1].Add(msgText);
            }
        }
        
    }
}