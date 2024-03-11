using UnityEngine;

public enum MessageType
{
    instant,
    delayed,
    afterMessageCount,
    random
}

[CreateAssetMenu(fileName = "Data", menuName = "MessageItem", order = 1)]
public class MessageItem : ScriptableObject
{
    // Internal ID for message, 6 digits
    // TODO: Determine organization scheme, ex 001122 00 = which module, 11 = which message line, 22 = which message in line 
    [SerializeField] public int MESSAGE_ID;
    [TextArea]
    [SerializeField] public string message;
    [Tooltip("CHECK IF GREEN IS CORRECT")]
    [SerializeField] public bool correct;
    [Tooltip("If this is checked, either answer is correct, the user still has to answer")]
    [SerializeField] public bool noFail;
    [Tooltip("The amount of damage the user takes if the request is failed")]
    [SerializeField] public int failPoints;
    [Tooltip("The amount of time it takes to fail the message")]
    [SerializeField] public float timeToFail;

    [SerializeField] public MessageType messageType;
    [Tooltip("Message type will come into effect when all flags are popped")]
    [SerializeField] public Flags[] flagsRequired;
    [Tooltip("The message will be sent again and again. If unchecked it will send ONE TIME")]
    [SerializeField] public bool repeat;
    [Tooltip("The number of messages it waits to send this message")]
    [SerializeField] public int messageWaitCount;
    [Tooltip("Time delay before sending this message")]
    [SerializeField] public float delayBeforeSending;

    [Tooltip("Only check if accept and decline raise flag are set")]
    [SerializeField] public bool completeRaiseFlag;
    [SerializeField] public Flags acceptRaiseFlag, declineRaiseFlag;

    [Tooltip("Only check if you have set messages to follow")]
    [SerializeField] public bool following;
    [SerializeField] public MessageItem acceptFollowingMessage, declineFollowingMessage;

    [SerializeField] public bool reblocked = false;




}