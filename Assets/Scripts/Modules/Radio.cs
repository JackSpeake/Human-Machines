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
}
