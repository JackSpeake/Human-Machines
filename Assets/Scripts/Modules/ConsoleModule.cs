using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleModule : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField input;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && input.text != "")
        {
            RunInput(input.text);
            input.text = "";
        }
    }

    // Mostly cheats
    private void RunInput(string s)
    {
        switch (s)
        {
            case "ilovemoney":
                GameManager.Instance.money += 500;
                break;
            case "sickday":
                GameManager.Instance.time += 100000;
                break;
            case "vacation":
                GameManager.Instance.day = 5;
                break;
            case "invincible":
                GameManager.Instance.hp += 100000;
                break;
            case "endgame":
                GameManager.Instance.day = 5;
                GameManager.Instance.stage = 3;
                break;
            default:
                break;
        }
    }
}
