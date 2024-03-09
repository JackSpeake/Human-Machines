using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RandomMinimizeVirus : VirusEffectA
{
    private Module module;
    private string[] taunts = { "Minimized in low.", "Have fun reopening those tabs, loser.", "Hope I didn't break your concentration <3." };
    public override void RunEffect()
    {
        if (module.open)
        {
            GameManager.Instance.SendEvilNotification(taunts[Random.Range(0, taunts.Length)]);
            module.Close();
        }
        else
            GameManager.Instance.SendEvilNotification("Wow, you really had NO modules open for me to minimize... Boring");
    }

    // Start is called before the first frame update
    void Start()
    {
        module = GetComponentInParent<Module>();
    }
}
