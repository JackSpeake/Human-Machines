using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text time, day, stage;
    GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan t = TimeSpan.FromSeconds(gm.lengthOfDay - gm.time);
        time.text = t.ToString("hh':'mm':'ss"); // 00:03:48

        day.text = gm.day.ToString();
        stage.text = gm.stage.ToString();
    }
}
