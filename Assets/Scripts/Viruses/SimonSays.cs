using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private Color[] buttonOffColors, buttonOnColors;
    [SerializeField] private Image[] buttons;
    [SerializeField] private Color victoryLockoutColor, failureLockoutColor;

    [SerializeField] private int numberOfBeeps = 4;
    private int[] order;
    private int playerIndex = 0;

    bool pressed = false;
    bool playingExample = false;
    bool lockout = false;
    float timeSincePress = 0;
    private bool mute = true;

    [SerializeField] private float pressedTime = 3f;
    [SerializeField] private float buttonFlashTime = .2f;
    [SerializeField] private float betweenButtonDelay = .5f;
    [SerializeField] private float betweenExampleDelay = 2f;
    [SerializeField] private float lockoutTime = 2f;

    [SerializeField] private VirusController vc;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] tracks;


    // Start is called before the first frame update
    void Start()
    {
        source.clip = tracks[0];
        int i = 0;
        foreach (Image b in buttons)
        {
            b.color = buttonOffColors[i];
            i++;
        }

        order = new int[numberOfBeeps];
        SetOrder();
    }

    void SetOrder()
    {
        for (int i = 0; i < numberOfBeeps; i++)
        {
            order[i] = Random.Range(0, buttons.Length);
        }
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (!lockout)
        {
            pressed = true;
            timeSincePress = 0;
            StartCoroutine(ButtonFlash(buttonIndex));
            if (order[playerIndex] == buttonIndex)
            {
                playerIndex++;

                if (playerIndex >= 4)
                {
                    StartCoroutine(VictoryLockout());
                }
            }
            else
            {
                StartCoroutine(FailureLockout());
            }
            
        }
    }

    private IEnumerator VictoryLockout()
    {
        lockout = true;
        pressed = false;

        yield return new WaitForSeconds(buttonFlashTime * 2);


        foreach (Image b in buttons)
        {
            b.color = victoryLockoutColor;
        }

        yield return new WaitForSeconds(lockoutTime);

        playerIndex = 0;
        SetOrder();

        // SOLVE ONE VIRUS

        int i = 0;
        foreach (Image b in buttons)
        {
            b.color = buttonOffColors[i];
            i++;
        }

        // VICTORYYYYY
        vc.DisinfectOne();

        lockout = false;
    }

    private IEnumerator FailureLockout()
    {
        lockout = true;
        pressed = false;

        yield return new WaitForSeconds(buttonFlashTime * 2);

        foreach (Image b in buttons)
        {
            b.color = failureLockoutColor;
        }

        yield return new WaitForSeconds(lockoutTime);

        playerIndex = 0;
        SetOrder();

        int i = 0;
        foreach (Image b in buttons)
        {
            b.color = buttonOffColors[i];
            i++;
        }

        lockout = false;
    }

    private IEnumerator ButtonFlash(int buttonIndex)
    {
        float t = 0;
        source.clip = tracks[buttonIndex];
        if (!mute) {
            source.Play(0);
        }
        

        while (t < buttonFlashTime)
        {
            t += Time.deltaTime;
            if (t < buttonFlashTime)
            {
                buttons[buttonIndex].color = Color.Lerp(buttonOffColors[buttonIndex], buttonOnColors[buttonIndex], Mathf.PingPong(t / (buttonFlashTime / 2), 1));
                yield return new WaitForEndOfFrame();
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            timeSincePress += Time.deltaTime;

            if (timeSincePress > pressedTime)
            {
                pressed = false;
                StartCoroutine(FailureLockout());
            }
        }
        else
        {
            timeSincePress = 0;

            if (!playingExample && !lockout)
            {
                StartCoroutine(ExamplePattern());
            }
        }
    }

    private IEnumerator ExamplePattern()
    {

        playingExample = true;

        yield return new WaitForSeconds(betweenExampleDelay);

        int i = 0;
        float t = 0;
        bool sfxplayed = false;

        while (!pressed && i < 4)
        {
            if (!sfxplayed) {
                source.clip = tracks[order[i]];
                if (!mute) {
                    source.Play(0);
                }
                sfxplayed = true;
            }
            t += Time.deltaTime;
            if (t < buttonFlashTime)
            {
                buttons[order[i]].color = Color.Lerp(buttonOffColors[order[i]], buttonOnColors[order[i]], Mathf.PingPong(t / (buttonFlashTime / 2), 1));
            }
            else
            {
                t = 0;
                i++;
                yield return new WaitForSeconds(betweenButtonDelay);
                sfxplayed = false;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(betweenExampleDelay);

        playingExample = false;
    }

    private void OnDisable()
    {
        playingExample = false;
        int i = 0;
        foreach (Image b in buttons)
        {
            b.color = buttonOffColors[i];
            i++;
        }
        pressed = false;
    }

    public void SetMute(bool status) {
        mute = status;
    }
}
