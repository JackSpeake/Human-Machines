using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource trackYapper;
    private AudioSource trackKeyboard;

    public NotificationArea notificationArea;
    public TMPro.TMP_Text headerText, lowerText;

    private void Start()
    {
        trackYapper = GetComponents<AudioSource>()[0];
        trackKeyboard = GetComponents<AudioSource>()[1];

        GetComponents<AudioSource>()[3].PlayDelayed(0.3f);
    }

    void Update ()
    {
        if (notificationArea != null)
        {
            if (notificationArea.messageText.maxVisibleCharacters < notificationArea.messageText.text.Length - 1
                && !trackYapper.isPlaying)
            {
                trackYapper.time = Random.Range(0f, 8f);
                trackYapper.Play(0);
            }
            else if (notificationArea.messageText.maxVisibleCharacters >= notificationArea.messageText.text.Length - 1)
                trackYapper.Stop();
        }
        else if (notificationArea == null && trackYapper.isPlaying)
        {
            trackYapper.Stop();
        }

        if (headerText != null)
        {
            if ((headerText.maxVisibleCharacters < headerText.text.Length - 1
                || lowerText.maxVisibleCharacters < lowerText.text.Length - 1)
                && !trackKeyboard.isPlaying)
            {
                trackKeyboard.Play(0);
            }
            else if (headerText.maxVisibleCharacters >= headerText.text.Length - 1
                && lowerText.maxVisibleCharacters >= lowerText.text.Length - 1)
                trackKeyboard.Stop();
        }
        else if (headerText == null && trackKeyboard.isPlaying)
        {
            trackKeyboard.Stop();
        }
    }
}
