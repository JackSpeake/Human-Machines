using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeShootoutVirus : VirusEffectA
{
    private string[] taunts = { "Hope you've been working on your aim, cowboy.", "Really? You can't beat a bunny?", "Red sun out today, who put that there... I wonder." };

    public override void RunEffect()
    {
        GetComponentInParent<Shootout>().hardMode = true;
        GameManager.Instance.SendEvilNotification(taunts[Random.Range(0, taunts.Length)]);
    }

}
