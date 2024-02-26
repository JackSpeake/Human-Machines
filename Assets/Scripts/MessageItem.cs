using UnityEngine;

public enum MessageType
{
    instant,
    delayed,
    afterMessageCount
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
    [SerializeField] public bool noFail;
    [SerializeField] public int failPoints;
    [SerializeField] public float timeToFail;

    [SerializeField] public MessageType messageType;
    [SerializeField] public Flags[] flagsRequired;
    [SerializeField] public bool repeat;
    [SerializeField] public int messageWaitCount;
    [SerializeField] public float delayBeforeSending;

    [SerializeField] public bool completeRaiseFlag;
    [SerializeField] public Flags acceptRaiseFlag, declineRaiseFlag;

    [SerializeField] public bool following;
    [SerializeField] public MessageItem acceptFollowingMessage, declineFollowingMessage;

    [SerializeField] public bool reblocked = false;




}