using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageItemManager : MonoBehaviour
{

    [SerializeField] private MessageItem[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        ResetReblockStatus();
    }

    public void ResetReblockStatus() {
        foreach (MessageItem item in items) {
            item.reblocked = false;
        }
    }
}
