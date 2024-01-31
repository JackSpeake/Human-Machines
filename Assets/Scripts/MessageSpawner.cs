using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    // THIS IS A TEMPORARY PROOF OF CONCEPT IMPLEMENTATION!
    // WILL BE CHANGED FUNCTIONALLY

    [Tooltip("The wait time until the next message is spawned.")]
    [SerializeField] private int minWaitTime, maxWaitTime;
    [SerializeField] private GameObject messagePrefab;

    private float timeSinceSpawnedMessage = 0.0f;
    [SerializeField] private List<MessageItem> availableMessageItems;
    private float currWaitTime;
    

    private void Start()
    {
        currWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawnedMessage += Time.deltaTime;

        if (timeSinceSpawnedMessage > currWaitTime)
        {
            currWaitTime = Random.Range(minWaitTime, maxWaitTime);
            timeSinceSpawnedMessage = 0;
            Instantiate(messagePrefab, this.transform).GetComponent<MessageObject>().SetMessageItem(availableMessageItems[Random.Range(0, availableMessageItems.Count)]);
        }
    }
}
