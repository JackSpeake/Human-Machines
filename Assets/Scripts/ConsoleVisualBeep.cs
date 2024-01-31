using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleVisualBeep : MonoBehaviour
{
    private TMPro.TMP_Text placeholder;
    [SerializeField] private float beepRate = .5f;

    private float timePassed = 0;

    private void Start()
    {
        placeholder = this.GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        
        if (timePassed > beepRate)
        {
            timePassed = 0;
            if (placeholder.text == "_")
            {
                placeholder.text = "";
            }
            else
            {
                placeholder.text = "_";
            }
        }
    }
}
