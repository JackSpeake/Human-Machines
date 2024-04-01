using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    // THIS IS A TEMPORARY PROOF OF CONCEPT IMPLEMENTATION!
    // WILL BE CHANGED FUNCTIONALLY

    [Tooltip("The wait time until the next message is spawned.")]
    [SerializeField] private int minWaitTime, maxWaitTime;
    [Tooltip("The prefab of the literal message being spawned.")]
    [SerializeField] private GameObject messagePrefab;

    private float timeSinceSpawnedMessage = 0.0f;

    private List<MessageItem> availableRandomMessageItems;
    [Tooltip("A list of messages that are reliant on flags to be sent.")]
    [SerializeField] public List<MessageItem> spawnOnFlagMessages;

    [SerializeField] private TimeoutModule tmModule;

    private List<(MessageItem, int)> messageCountSendQueue;

    public bool spawning = true;

    private float currWaitTime;

    private void Start()
    {
        currWaitTime = Random.Range(minWaitTime, maxWaitTime);
        messageCountSendQueue = new List<(MessageItem, int)>();
        availableRandomMessageItems = new List<MessageItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning && GameManager.Instance.started && !tmModule.paused)
        {
            SpawnFlagMessage();
            SpawnRandom();
        }
    }

    public void ResetMessages()
    {
        messageCountSendQueue.Clear();
        availableRandomMessageItems.Clear();
    }

    // Just spawns the message attached
    public void SpawnMessage(MessageItem message)
    {
        // If instant, send message
        if (message.messageType == MessageType.instant)
        {
            Instantiate(messagePrefab, this.transform).GetComponent<MessageObject>().SetMessageItem(message);

            updateMessageCountMessages();
        }
        // If delayed by time, start coroutine
        else if (message.messageType == MessageType.delayed)
        {
            StartCoroutine(DelayedMessage(message));
            
        }
        // If delayed by message count, save for later.
        else if (message.messageType == MessageType.afterMessageCount)
        {
            messageCountSendQueue.Add((message, message.messageWaitCount));
        }
        else if (message.messageType == MessageType.random)
        {
            availableRandomMessageItems.Add(message);
            
        }
    }

    // Coroutine that waits to spawn a delayed message.
    IEnumerator DelayedMessage(MessageItem m)
    {
        // suspend execution for x seconds
        yield return new WaitForSeconds(m.delayBeforeSending);
        Instantiate(messagePrefab, this.transform).GetComponent<MessageObject>().SetMessageItem(m);
        updateMessageCountMessages();
    }

    // This updates the queue of messages waiting for a certain number of messages to be sent
    void updateMessageCountMessages()
    {
        for (int i = 0; i < messageCountSendQueue.Count; i++)
        {
            messageCountSendQueue[i] = (messageCountSendQueue[i].Item1, messageCountSendQueue[i].Item2 - 1);
        }

        foreach ((MessageItem, int) m in messageCountSendQueue)
        {
            if (m.Item2 == 0)
            {
                Instantiate(messagePrefab, this.transform).GetComponent<MessageObject>().SetMessageItem(m.Item1);
                
                messageCountSendQueue.Remove(m);
            }
        }
    }

    // Runs once per frame, checks messages with flags attached then deals with them accordingly.
    void SpawnFlagMessage()
    {
        foreach (MessageItem m in spawnOnFlagMessages.ToArray())
        {
            if (SetFlags.containsAllFlags(m.flagsRequired) || m.flagsRequired.Length == 0)
            {
                if (m.flagsNotAllow.Length == 0 || SetFlags.containsNoFlags(m.flagsNotAllow))
                {
                    //if (m.messageType == MessageType.random)
                    //  Debug.Log("Made it here");
                    SpawnMessage(m);

                    if (m.repeat == false || m.messageType == MessageType.random)
                    {
                        spawnOnFlagMessages.Remove(m);
                    }
                }
            }
        }
    }

    // Runs every frame
    // Checks if a random amount of time has elapsed, if so, send message from list of random messages
    void SpawnRandom()
    {
        timeSinceSpawnedMessage += Time.deltaTime;

        if (availableRandomMessageItems.Count > 0)
            Debug.Log("Available messages: " + availableRandomMessageItems.Count);

        if (timeSinceSpawnedMessage > currWaitTime && availableRandomMessageItems.Count > 0)
        {
            currWaitTime = Random.Range(minWaitTime, maxWaitTime);
            timeSinceSpawnedMessage = 0;
            MessageItem m = availableRandomMessageItems[Random.Range(0, availableRandomMessageItems.Count)];
            Instantiate(messagePrefab, this.transform).GetComponent<MessageObject>().SetMessageItem(m);
            if (!m.repeat)
                availableRandomMessageItems.Remove(m);
            updateMessageCountMessages();
        }
    }
}
