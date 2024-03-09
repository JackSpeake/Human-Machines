using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirusEffectA : MonoBehaviour
{
    [SerializeField] public float healthThreshold;

    public abstract void RunEffect();
}
