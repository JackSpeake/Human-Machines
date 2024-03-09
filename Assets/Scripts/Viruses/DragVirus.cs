using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragVirus : VirusEffectA
{
    private Module module;

    [SerializeField] private float dragSpeed;
    [SerializeField] private float dragTime;
    [SerializeField] private int dragCount;

    private string[] taunts = { "HAHAHAHAHAHA", "You just got DRAGGED", "Oh, were you using those?" };

    public override void RunEffect()
    {
        StartCoroutine(ModuleDrag());
        GameManager.Instance.SendEvilNotification(taunts[Random.Range(0, taunts.Length)]);
    }

    // Start is called before the first frame update
    void Start()
    {
        module = GetComponentInParent<Module>();
    }

    private IEnumerator ModuleDrag()
    {
        float t = 0;
        Transform move = module.transform;

        for (int i = 0; i < dragCount; i++)
        {
            Vector3 dir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);

            while (t < dragTime)
            {
                t += Time.deltaTime;
                move.Translate(dir * dragSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            t = 0;
        }
        
    }
}
