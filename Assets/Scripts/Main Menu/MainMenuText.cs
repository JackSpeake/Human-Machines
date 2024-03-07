using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
    [SerializeField] private float timeWait = .75f;

    private float t;
    private TMPro.TMP_Text text;
    int count = 0;

    private void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > timeWait)
        {
            t = 0;

            switch (count)
            {
                case 0:
                    text.text = "HUMAN\nMACHINES";
                    break;
                case 1:
                    text.text = "HUMAN\nMACHINES.";
                    break;
                case 2:
                    text.text = "HUMAN\nMACHINES..";
                    break;
                case 3:
                    text.text = "HUMAN\nMACHINES...";
                    break;
            }

            count++;
            if (count > 3)
            {
                count = 0;
            }
        }
    }
}
