using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursedTextVirus : VirusEffectA
{
    [TextArea]
    [SerializeField] private string[] possibleTexts;
    [SerializeField] private float effectLength;
    [SerializeField] private int effectSpeed;
    [SerializeField] private int effectStartStopSpeedup;

    private TMPro.TMP_Text text;
    private string[] taunts = { "Were you reading that?", "THE CORRUPTION COMPELLS ME.", "You better get on disinfecting that computer...", "Your fight only compells me further." };

    private void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    public override void RunEffect()
    {
        StartCoroutine(StartEffect());
        
        GameManager.Instance.SendEvilNotification(taunts[UnityEngine.Random.Range(0, taunts.Length)]);
    }

    private IEnumerator StartEffect()
    {
        float t = 0;
        text.text = possibleTexts[UnityEngine.Random.Range(0, possibleTexts.Length)];
        text.maxVisibleCharacters = 0;

        while (text.maxVisibleCharacters < text.text.Length)
        {
            text.maxVisibleCharacters += effectStartStopSpeedup;
            text.text = leftrotate(text.text, effectSpeed);
            yield return new WaitForEndOfFrame();
        }

        while (t < effectLength)
        {
            text.text = leftrotate(text.text, effectSpeed);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        text.alignment = TMPro.TextAlignmentOptions.Right;

        while (text.text.Length > effectStartStopSpeedup)
        {
            text.text = text.text.Substring(0, text.text.Length - effectStartStopSpeedup);
            text.text = leftrotate(text.text, effectSpeed);
            yield return new WaitForEndOfFrame();
        }

        text.text = "";
    }

    static String leftrotate(String str, int d)
    {
        String ans = str.Substring(d, str.Length - d) + str.Substring(0, d);
        return ans;
    }
}
