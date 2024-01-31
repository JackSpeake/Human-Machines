using UnityEngine;

public enum MessageType
{
    instant,
    delayed,
    afterMessageCount
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MessageItem", order = 1)]
public class MessageItem : ScriptableObject
{
    // Internal ID for message, 6 digits
    // TODO: Determine organization scheme, ex 001122 00 = which module, 11 = which message line, 22 = which message in line 
    [SerializeField] public int MESSAGE_ID;
    [SerializeField] public string message;

    [SerializeField] public MessageType messageType;
    [SerializeField] public Flags[] flagsRequired;
    [SerializeField] public bool repeat;
    [SerializeField] public bool inSequence;
    [SerializeField] public MessageItem followingMessage;
    [SerializeField] public int messageWaitCount;
    [SerializeField] public float delayBeforeSending;
}