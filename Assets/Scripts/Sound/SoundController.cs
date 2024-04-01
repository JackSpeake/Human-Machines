using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public bool play1 = false;
    private AudioSource track1;
    
    void Start ()
    {
        track1 = GetComponent<AudioSource>();
        if (play1)
        {
            track1.Play(0);
        }
    }

    void playTrack()
    {
        track1.Play(0);
    }

    void stopTrack()
    {
        track1.Stop();
    }
}
