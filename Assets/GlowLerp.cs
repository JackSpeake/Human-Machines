using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlowLerp : MonoBehaviour
{
    [SerializeField] private Material m;
    [SerializeField] private float lerpTime = .1f;
    [SerializeField] private float lowFloat, highFloat;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color startColor;

    private SpriteRenderer sr;

    private void Start()
    {
        m.SetColor("_EmissionColor", startColor * 0);
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void ToggleGlow(bool on)
    {
        StartCoroutine(ToggleGlowCoroutine(on));
    }

    private IEnumerator ToggleGlowCoroutine(bool on)
    {
        float t = 0;
        m.EnableKeyword("_EMISSION");

        if (on)
        {
            sr.enabled = true;
            while (t < lerpTime)
            {
                m.SetColor("_EmissionColor", baseColor * Mathf.Lerp(lowFloat, highFloat, t / lerpTime));
                t += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }

            m.SetColor("_EmissionColor", baseColor * highFloat);
        }

        else
        {
            while (t < lerpTime)
            {
                m.SetColor("_EmissionColor", baseColor * Mathf.Lerp(highFloat, lowFloat, t / lerpTime));
                t += Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }

            m.SetColor("_EmissionColor", baseColor * lowFloat);
            sr.enabled = false;
        }

    }
}
