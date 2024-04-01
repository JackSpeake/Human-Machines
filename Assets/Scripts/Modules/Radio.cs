using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public float volume = 100.0f;
    
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] tracks;

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
