using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthModulePanel : MonoBehaviour
{
    public Image panel;
    public UnityEngine.UI.Extensions.Gradient2 gradient;
    public bool infected = false, infecting = false, disinfecting = false;
    public int x, y;

    [SerializeField] public static float infectionSpeed, disInfectionSpeed;

    private void Start()
    {
        panel = GetComponent<Image>();
        gradient = GetComponent<UnityEngine.UI.Extensions.Gradient2>();
    }

    private void Update()
    {
    }

    public void UpdateStaticVariables(float infSpeed, float disInfSpeed)
    {
        infectionSpeed = infSpeed;
        disInfectionSpeed = disInfSpeed;
    }

    public void StartInfection()
    {
        if (!infecting && !infected)
        {
            infecting = true;
            disinfecting = false;
            StartCoroutine(StartInfectionCoroutine());
        }
        
    }

    public IEnumerator StartInfectionCoroutine()
    {
        float t = 0;
        float startOffset = gradient.Offset;

        while (infecting && t < infectionSpeed)
        {
            gradient.Offset = Mathf.Lerp(startOffset, -1, t / infectionSpeed);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        if (infecting && !disinfecting)
        {
            infecting = false;
            infected = true;
        }
    }

    public void Disinfect()
    {
        if (!disinfecting && infected)
        {
            infecting = false;
            disinfecting = true;
            StartCoroutine(StartDisinfectionCoroutine());
        }
            
    }

    public IEnumerator StartDisinfectionCoroutine()
    {
        float t = 0;

        float startOffset = gradient.Offset;

        while (disinfecting && t < disInfectionSpeed)
        {
            gradient.Offset = Mathf.Lerp(startOffset, 1, t / disInfectionSpeed);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        if (disinfecting && !infecting)
        {
            disinfecting = false;
            infected = false;
        }
    }
}
