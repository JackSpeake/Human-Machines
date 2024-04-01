using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{  
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private int track_num = 0;
    [SerializeField] private Transform dialTransform;

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
        dialTransform.rotation = Quaternion.Euler(0f, 0f, -140f);
    }

    public void VolumeFive() {
        source.volume = 0.83f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
    }

    public void VolumeFour() {
        source.volume = 0.66f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, -49f);
    }

    public void VolumeThree() {
        source.volume = 0.5f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void VolumeTwo() {
        source.volume = 0.33f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, 49f);
    }

    public void VolumeOne() {
        source.volume = 0.16f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public void VolumeOff() {
        source.volume = 0.0f;
        dialTransform.rotation = Quaternion.Euler(0f, 0f, 140f);
    }
}
