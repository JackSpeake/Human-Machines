using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilYapperEffect : VirusEffectA
{
    [SerializeField] private string[] insults;

    public override void RunEffect()
    {
        GameManager.Instance.SendEvilNotification(insults[Random.Range(0, insults.Length)]);
    }
}
