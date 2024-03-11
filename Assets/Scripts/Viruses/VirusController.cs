using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will control all references to system health and virus spawning
public class VirusController : MonoBehaviour
{
    [SerializeField] private PlayerHealthModule HealthModule;

    [Range(3f, 20f)]
    [Tooltip("Range of how many seconds between infection attempts")]
    [SerializeField] private float infectionSpeedMin = 5f;

    [Range(20f, 60f)]
    [Tooltip("Range of how many seconds between infection attempts")]
    [SerializeField] private float infectionSpeedMax = 15f;

    [Tooltip("The limit of how many panels 1 infection can spread to during an infection spread")]
    [SerializeField] private int infectionSpreadMax = 1;

    [Range(0.0f, 1f)]
    [Tooltip("The odds of an infection spreading during an infection")]
    [SerializeField] private float infectionLikelyhood = 1;

    [Range(0.0f, 1f)]
    [Tooltip("The odds of a new infection starting")]
    [SerializeField] private float newInfectionLikelyhood = 1;

    [SerializeField] private string[] firstHealthOpenInstructions;

    // SETS THE STATIC VARIABLES INSIDE THE PLAYER HEALTH PANELS
    [SerializeField] private float infectionSpeed, disInfectionSpeed;

    private int infectedPanelCount = 0;
    private float infectionPercent;

    [Tooltip("If 1, infectionPercent chance of a virus every 1 second")]
    [SerializeField] private int virusEffectsTryRate = 10;

    float t = 0;
    float tVirus = 0;
    float currInfectionSpeed = 0;

    bool staticValuesUpdated = false;

    private int shieldLevel = 0;

    private bool firstEnable = true;

    private void Start()
    {
        currInfectionSpeed = Random.Range(infectionSpeedMin, infectionSpeedMax);
        t = 0;
        infectedPanelCount = 0;
        
    }

    private void Update()
    {
        if (HealthModule && HealthModule.gameObject.activeInHierarchy)
        {
            if (firstEnable)
            {
                foreach (string s in firstHealthOpenInstructions)
                    GameManager.Instance.SendNotification(s);
            }

            firstEnable = false;
            infectionPercent = ((float) infectedPanelCount) / 25f * 100f;
            if (!staticValuesUpdated)
            {
                HealthModule.GetCenterPanel().UpdateStaticVariables(infectionSpeed, disInfectionSpeed);
                staticValuesUpdated = true;
            }

            t += Time.deltaTime;
            tVirus += Time.deltaTime;

            if (tVirus >= virusEffectsTryRate)
            {
                tVirus = 0;

                if (Random.Range(0, 100) < infectionPercent)
                {
                    SendVirusEffect();
                }
            }

            if (t >= currInfectionSpeed)
            {
                InfectPanels();
                t = 0;
            }

            if (HealthModule.GetCenterPanel().infected)
            {
                GameOver();
            }
        }
    }

    // not efficient (do we care?)
    private void SendVirusEffect()
    {
        GameObject[] viruses = GameObject.FindGameObjectsWithTag("Virus");
        Debug.Log("Sending Virus Effect");
        viruses[Random.Range(0, viruses.Length)].GetComponent<VirusEffectA>().RunEffect();
    }

    private void GameOver()
    {
        GameManager.Instance.hackLose = true;
        GameManager.Instance.Lose();
    }

    public void addShield(int shieldAmount)
    {
        shieldLevel += shieldAmount;
        HealthModule.SetShieldLevel(shieldLevel);
    }

    public void removeShield(int shieldAmount)
    {
        shieldLevel -= shieldAmount;
        HealthModule.SetShieldLevel(shieldLevel);
    }

    private void InfectPanels()
    {
        List<HealthModulePanel> infectedPanels = HealthModule.GetInfectedPanels();
        infectedPanelCount = infectedPanels.Count;

        if (infectedPanelCount > 0 && Random.Range(0f, 100f) / 100 > newInfectionLikelyhood)
        {
            foreach (HealthModulePanel h in infectedPanels)
            {
                int infectCount = 0;
                IList<HealthModulePanel> neighbors = HealthModule.GetNeighborsOfXY(h.x, h.y).Shuffle();

                foreach (HealthModulePanel n in neighbors)
                {
                    if (Random.Range(0f, 100f) / 100 > infectionLikelyhood && !n.infecting && !n.infected)
                    {
                        if (shieldLevel > 0)
                            removeShield(1);
                        else
                            n.StartInfection();
                        infectCount++;
                    }

                    if (infectCount >= infectionSpreadMax)
                        break;
                }
            }
        }
        else
        {
            InfectFirstPanel();
        }
    }

    private void InfectFirstPanel()
    {
        IList<HealthModulePanel> edgePanels = HealthModule.GetOutside().Shuffle();

        if (Random.Range(0f, 100f) / 100 > infectionLikelyhood )
        {
            HealthModulePanel h;
            bool infected = false;
            do
            {
                h = edgePanels[Random.Range(0, edgePanels.Count - 1)];
                if (!h.infecting && !h.infected)
                {
                    if (shieldLevel > 0)
                        removeShield(1);
                    else
                        h.StartInfection();
                    infected = true;
                }
            } while (!infected);
        }
    }

    public void DisinfectOne()
    {
        List<HealthModulePanel> infectedPanels = HealthModule.GetInfectedPanels();

        int rand = Random.Range(0, infectedPanels.Count - 1);

        if (infectedPanels.Count > 0)
            infectedPanels[rand].Disinfect();
    }
}
