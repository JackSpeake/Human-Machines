using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnFlag : MonoBehaviour
{
    [SerializeField] private GameObject toEnable;
    [SerializeField] private Flags enableFlag;

    private void Update()
    {
        if (SetFlags.activeFlags.Contains(enableFlag))
        {
            toEnable.gameObject.SetActive(true);
            Destroy(this);
        }
            
    }
}
