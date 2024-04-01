using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{  
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private int track_num = 0;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = tracks[0];
        source.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTrack() {
        track_num = (track_num + 1 ) % tracks.Length;
        source.clip = tracks[track_num];
        source.Play(0);
    }

    public void PrevTrack() {
        track_num -= 1;
        if (track_num < 0) {
            track_num = tracks.Length - 1;
        }

        source.clip = tracks[track_num];
        source.Play(0);
    }

    public void VolumeMax() {
        source.volume = 1.0f;
    }

    public void VolumeFive() {
        source.volume = 0.83f;
    }

    public void VolumeFour() {
        source.volume = 0.66f;
    }

    public void VolumeThree() {
        source.volume = 0.5f;
    }

    public void VolumeTwo() {
        source.volume = 0.33f;
    }

    public void VolumeOne() {
        source.volume = 0.16f;
    }

    public void VolumeOff() {
        source.volume = 0.0f;
    }
}
