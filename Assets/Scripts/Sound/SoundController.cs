using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource trackYapper;
    private AudioSource trackKeyboard;

    public NotificationArea notificationArea;

    private void Start()
    {
        trackYapper = GetComponent<AudioSource>();
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
            else if (notificationArea.messageText.maxVisibleCharacters >= notificationArea.messageText.text.Length)
                trackYapper.Stop();
        }
    }
}
