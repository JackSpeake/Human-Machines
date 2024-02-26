using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    [SerializeField] private KeyCode speedUpKey;
    [SerializeField] private float speedUpRate = 5f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(speedUpKey))
        {
            Time.timeScale = speedUpRate;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
