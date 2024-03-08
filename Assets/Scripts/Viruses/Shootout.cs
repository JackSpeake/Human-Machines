using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shootout : MonoBehaviour
{
    // VISUAL STUFF
    private Vector3 blOrigin, brOrigin, groundOrigin, enemyOrigin, handOrigin, sunOrigin;
    [SerializeField] public Transform buildingsLeft, buildingsRight, ground, enemy, hand, sun;

    [SerializeField] private float moveInTime, sunRiseTime;
    [SerializeField] private float enemyWaitDelay, sunWaitDelay, drawWaitDelay;
    [SerializeField] private float fallOverTimeWin, delayBeforeExit, itemExitSpeed, bodyExitTime, victoryTextTime;
    [SerializeField] private TMPro.TMP_Text victoryText;

    [SerializeField] private Button drawButton;

    [SerializeField] private GameObject gunShot;
    [SerializeField] private Image panel;

    // GAMEPLAY STUFF
    [SerializeField] private float LoseTime, minGameStartTime, maxGameStartTime;
    [SerializeField] private VirusController vc;
    [SerializeField] private int shieldAmount = 20;

    float timeForGameStart = 0;
    float currGameWaitTime;
    bool shot = false;
    bool inProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("YO?");
        SavePositions();
        //Leave();
        drawButton.gameObject.SetActive(false);
        victoryText.gameObject.SetActive(false);
        currGameWaitTime = Random.Range(minGameStartTime, maxGameStartTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress)
            timeForGameStart += Time.deltaTime;

        if (timeForGameStart > currGameWaitTime)
        {
            Leave();
            timeForGameStart = 0;
            currGameWaitTime = Random.Range(minGameStartTime, maxGameStartTime);
            StartCoroutine(ReturnToCenterCoroutine());
            inProgress = true;
        }
            
        
    }

    private void SavePositions()
    {
        blOrigin = buildingsLeft.localPosition;
        brOrigin = buildingsRight.localPosition;
        groundOrigin = ground.localPosition;
        enemyOrigin = enemy.localPosition;
        handOrigin = hand.localPosition;
        sunOrigin = sun.localPosition;

        buildingsLeft.gameObject.SetActive(false);
        buildingsRight.gameObject.SetActive(false);
        ground.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);
        sun.gameObject.SetActive(false);
    }

    private void Leave()
    {
        buildingsLeft.gameObject.SetActive(true);
        buildingsRight.gameObject.SetActive(true);
        ground.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        sun.gameObject.SetActive(true);

        buildingsLeft.Translate(new Vector3(-5, 0, 0));
        buildingsRight.Translate(new Vector3(5, 0, 0));
        ground.Translate(new Vector3(0, -5, 0));
        enemy.Translate(new Vector3(0, 5, 0));
        hand.Translate(new Vector3(5, 0, 0));
        sun.Translate(new Vector3(0, -5, 0));
    }

    private IEnumerator ReturnToCenterCoroutine()
    {
        float t = 0;

        Vector3 startPos = buildingsLeft.localPosition;
        while (t < moveInTime)
        {
            buildingsLeft.localPosition = Vector3.Lerp(startPos, blOrigin, t / moveInTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsRight.localPosition;
        while (t < moveInTime)
        {
            buildingsRight.localPosition = Vector3.Lerp(startPos, brOrigin, t / moveInTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsRight.localPosition;
        while (t < moveInTime)
        {
            buildingsRight.localPosition = Vector3.Lerp(startPos, brOrigin, t / moveInTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = ground.localPosition;
        while (t < moveInTime)
        {
            ground.localPosition = Vector3.Lerp(startPos, groundOrigin, t / moveInTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = hand.localPosition;
        while (t < moveInTime)
        {
            hand.localPosition = Vector3.Lerp(startPos, handOrigin, t / moveInTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(enemyWaitDelay);

        startPos = enemy.localPosition;
        while (t < sunRiseTime)
        {
            enemy.localPosition = Vector3.Lerp(startPos, enemyOrigin, t / sunRiseTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(sunWaitDelay);

        startPos = sun.localPosition;
        while (t < sunRiseTime)
        {
            sun.localPosition = Vector3.Lerp(startPos, sunOrigin, t / sunRiseTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(drawWaitDelay);
        DrawButton();

    }

    private void DrawButton()
    {
        buildingsLeft.gameObject.SetActive(false);
        buildingsRight.gameObject.SetActive(false);
        ground.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);
        sun.gameObject.SetActive(false);

        drawButton.gameObject.SetActive(true);

        StartCoroutine(FailTimer());
    }

    private IEnumerator FailTimer()
    {
        float t = 0;
        TMPro.TMP_Text buttonText = drawButton.GetComponentInChildren<TMPro.TMP_Text>();
        while (!shot && t <= LoseTime )
        {
            buttonText.color = Color.Lerp(Color.white, Color.red, t / LoseTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (!shot)
        {
            buildingsLeft.gameObject.SetActive(true);
            buildingsRight.gameObject.SetActive(true);
            ground.gameObject.SetActive(true);
            enemy.gameObject.SetActive(true);
            sun.gameObject.SetActive(true);
            drawButton.gameObject.SetActive(false);
            panel.color = Color.red;

            StartCoroutine(FailState());
        }
        // GUNSHOT NOISE

        buttonText.color = Color.white;

        
    }

    private IEnumerator FailState()
    {
        float t = 0;
        yield return new WaitForSeconds(fallOverTimeWin);

        Vector3 startPos = sun.localPosition;
        while (t < itemExitSpeed)
        {
            sun.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, 5, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = ground.localPosition;
        while (t < itemExitSpeed)
        {
            ground.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, -5, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsLeft.localPosition;
        while (t < itemExitSpeed)
        {
            buildingsLeft.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(-5, 0, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsRight.localPosition;
        while (t < itemExitSpeed)
        {
            buildingsRight.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(5, 0, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(delayBeforeExit);

        // GUNSHOT NOISE

        enemy.Translate(new Vector3(0, 5, 0));

        hand.gameObject.SetActive(true);
        hand.Translate(new Vector3(0, -5, 0));
        panel.color = Color.black;
        inProgress = false;
    }

    public void Shoot()
    {
        shot = true;

        buildingsLeft.gameObject.SetActive(true);
        buildingsRight.gameObject.SetActive(true);
        ground.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        sun.gameObject.SetActive(true);
        gunShot.gameObject.SetActive(true);

        drawButton.gameObject.SetActive(false);

        vc.addShield(shieldAmount);

        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        float t = 0;

        victoryText.gameObject.SetActive(true);

        yield return new WaitForSeconds(fallOverTimeWin);
        

        Vector3 startPos = enemy.localPosition;
        while (t < bodyExitTime)
        {
            enemy.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, -5, 0), t / bodyExitTime);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(delayBeforeExit);

        startPos = sun.localPosition;
        while (t < itemExitSpeed)
        {
            sun.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, 5, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = ground.localPosition;
        while (t < itemExitSpeed)
        {
            ground.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, -5, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsLeft.localPosition;
        while (t < itemExitSpeed)
        {
            buildingsLeft.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(-5, 0, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        startPos = buildingsRight.localPosition;
        while (t < itemExitSpeed)
        {
            buildingsRight.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(5, 0, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;
        
        startPos = hand.localPosition;
        while (t < itemExitSpeed)
        {
            hand.localPosition = Vector3.Lerp(startPos, startPos + new Vector3(0, -5, 0), t / itemExitSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t = 0;

        yield return new WaitForSeconds(victoryTextTime);

        victoryText.gameObject.SetActive(false);

        gunShot.gameObject.SetActive(false);

        shot = false;
        inProgress = false;
    }
}
