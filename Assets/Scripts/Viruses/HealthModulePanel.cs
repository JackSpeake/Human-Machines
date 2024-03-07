using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthModulePanel : MonoBehaviour
{
    public Image panel;
    public bool infected = false, infecting = false;
    public int x, y;

    private void Start()
    {
        panel = GetComponent<Image>();
    }

    public void StartInfection()
    {
        // LERP TO
    }

    public void Disinfect()
    {

    }
}
