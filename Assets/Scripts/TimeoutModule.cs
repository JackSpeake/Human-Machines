using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeoutModule : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text body;

    [SerializeField] private Image img;
    [SerializeField] private Button button;

    [SerializeField] private float cooldown;

    [SerializeField] public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0.0f;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (cooldown == 0.0f) {
            img.color = new Color(0.79f, 0.79f, 0.58f, 1.0f);
            body.text = "[ READY ]";
            button.enabled = true;
        }
        else if (cooldown <= 180.0f) {
            img.color = new Color(0.6f, 0.6f, 0.55f, 1.0f);
            button.enabled = false;
            body.text = "[ Reconfiguring . . .]\n[  Come back later  ]\n" + Mathf.Round(cooldown);
            paused = false;
        }
        else if (cooldown <= 200.0f) {
            img.color = new Color(0.44f, 0.44f, 0.34f, 1.0f);
            button.enabled = false;
            body.text = "[ INTERNET OUTAGE IN EFFECT ]\n" + Mathf.Clamp(Mathf.Round(cooldown - 180.0f), 0.0f, 20.0f);
            paused = true;
        }

        cooldown = Mathf.Clamp(cooldown - Time.deltaTime, 0.0f, 200f);
    }

    public void Timeout() {
        cooldown = 200.0f;

    }
}
